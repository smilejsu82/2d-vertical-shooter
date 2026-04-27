# 2D Vertical Shooter

Unity 2D 기반 세로 스크롤 슈팅 프로젝트입니다.
플레이어는 화면 경계 안에서 이동하며 총알을 발사하고, 적(A/B/C 타입)을 처치해 점수를 올립니다.
아이템을 획득하면 파워업·폭탄을 얻을 수 있으며, 폭탄 스킬로 화면의 적을 전부 제거할 수 있습니다.

## 프로젝트 개요

- 장르: 2D Vertical Shooter
- 엔진: Unity 6
- 핵심 플레이:
  - 플레이어 이동 + 연사
  - 랜덤 적 스폰(상단/사이드)
  - 적 타입별 속도/행동 차이
  - 아이템 드랍 및 획득 (Coin / Power / Boom)
  - 폭탄 스킬로 화면 내 적 전부 제거
  - 라이프 감소/리스폰/게임오버
  - 점수 누적 및 재시작
  - **오브젝트 풀링**으로 런타임 Instantiate/Destroy 제거

## 개발 환경

- Unity Editor: `6000.4.1f1`
- 렌더 파이프라인: URP (`com.unity.render-pipelines.universal`)
- 주요 패키지:
  - `com.unity.inputsystem` 1.19.0
  - `com.unity.ugui` 2.0.0
  - `com.unity.textmeshpro` (프로젝트 내 TextMesh Pro 리소스 포함)

참고 파일:
- `ProjectSettings/ProjectVersion.txt`
- `Packages/manifest.json`

## 실행 방법

1. Unity Hub에서 이 프로젝트 폴더를 엽니다.
2. Unity 버전을 `6000.4.1f1`로 맞춥니다.
3. 씬 `Assets/Scenes/GameScene.unity`를 엽니다.
4. Play 버튼으로 실행합니다.

## 조작법

- 이동: `Horizontal / Vertical` 축 입력 (기본 WASD/화살표)
- 발사: 마우스 왼쪽 버튼 누르고 있기 (`Input.GetMouseButton(0)`)
- 폭탄 스킬: 마우스 오른쪽 버튼 클릭 (`Input.GetMouseButtonDown(1)`)

## 게임 규칙

### 플레이어

- 플레이어는 화면 경계를 벗어나지 않도록 Clamp 처리됩니다.
- 발사 패턴은 `power` 값에 따라 바뀝니다.
  - `power = 1`: 중앙 단발
  - `power = 2`: 좌/우 2발
  - `power = 3`: 중앙 강화탄 + 좌/우 2발
- 피격 조건:
  - 적 본체(`Enemy` 태그) 충돌
  - 적 총알(`EnemyBullet` 태그) 충돌
- 피격 시 처리:
  1. UI 라이프 1 감소
  2. 플레이어 비활성화
  3. 게임오버가 아니면 `respawnDelay` 후 시작 위치로 재활성화

### 적

- 적은 `GameManager`에서 랜덤 생성됩니다.
  - 상단 스폰 후 아래 이동 또는
  - 사이드 스폰 후 지정 방향 이동
- 타입별 이동 속도(현재 코드 기본값):
  - C: 가장 느림 (`speedC = 1f`)
  - A: 중간 (`speedA = 2f`)
  - B: 가장 빠름 (`speedB = 3f`)
- C 타입은 주기적으로 총알 발사:
  - `firePoints[0]`, `firePoints[1]`에서 각각 1발
  - 플레이어 방향으로 유도 발사

### 아이템

적기 사망 시 확률적으로 아이템을 드랍합니다.

| 아이템 | 드랍 확률 | 효과 |
|---|---|---|
| None | 30% | 드랍 없음 |
| Coin | 30% | 스코어 +1000 |
| Power | 20% | 스코어 +500, 파워 레벨 +1 (최대 3) |
| Boom | 20% | 스코어 +500, 폭탄 +1 (최대 3) |

### 폭탄 스킬

- 마우스 오른쪽 버튼으로 폭탄 스킬 사용 (`boomCount > 0`일 때만 가능)
- 사용 시 화면 내 모든 적(`Enemy`)·적 총알(`EnemyBullet`) 즉시 풀로 반환
- `SkillBoomPrefab` 이펙트가 2초간 표시된 후 자동 제거
- 사용 후 `boomCount` 1 감소, UI 폭탄 아이콘 갱신

### 점수

- 적 처치 시 타입별 점수:
  - A: +100
  - B: +200
  - C: +300
- 아이템 획득 시:
  - Coin: +1000
  - Power: +500
  - Boom: +500

### 라이프/게임오버

- 라이프 아이콘(`Image[]`)의 alpha를 0으로 만들어 소모 표현
- 마지막 라이프 소모 시 GameOver 패널 활성화
- Retry 버튼으로 현재 씬 재로드

## 오브젝트 풀링

런타임 중 `Instantiate` / `Destroy` 호출을 제거하고, 씬 시작 시 미리 생성해둔 오브젝트를 재사용합니다.

### ObjectPoolManager

`ObjectPoolManager` (싱글톤)가 모든 풀을 관리합니다.

| 풀 | 프리팹 | 초기 수량 |
|---|---|---|
| playerBullet0 | PlayerBullet0Prefab | 20 |
| playerBullet1 | PlayerBullet1Prefab | 20 |
| enemyA | EnemyAPrefab | 10 |
| enemyB | EnemyBPrefab | 10 |
| enemyC | EnemyCPrefab | 20 |
| enemyBullet0 | EnemyBullet0Prefab | 20 |
| itemCoin | ItemCoinPrefab | 20 |
| itemPower | ItemPowerPrefab | 10 |
| itemBoom | ItemBoomPrefab | 10 |

### 풀 사용 규칙

- **꺼내기**: `Get~()` 메서드 → 위치 설정 → `SetActive(true)`
- **반환**: `ReleaseBullet()` / `ReleaseEnemy()` / `ReleaseItem()` 호출 (`SetActive(false)` + 위치 초기화)
- `Destroy()`를 직접 호출하면 리스트에 파괴된 참조가 남아 `MissingReferenceException`이 발생하므로 반드시 `Release~()` 사용

### 오브젝트별 활성화 순서

```
Enemy    : 위치 설정 → SetActive(true) [OnEnable: 상태 초기화] → StartMove()
EnemyBullet: 위치 설정 → StartMove() → SetActive(true)
PlayerBullet: 위치/회전 설정 → SetActive(true)
Item     : 위치 설정 → SetActive(true)
```

> Enemy는 `OnEnable()`에서 체력·이동 플래그를 초기화하기 때문에
> `SetActive(true)` 이후에 `StartMove()`를 호출해야 합니다.

## 씬/오브젝트 구성 (핵심)

메인 플레이 씬:
- `Assets/Scenes/GameScene.unity`

주요 오브젝트:
- `Player`
- `GameManager`
- `UIManager`
- `ObjectPoolManager`
- `AreaDrawer`
- `BackgroundManager`

주요 프리팹:
- `Assets/Prefabs/EnemyAPrefab.prefab` (`Enemy` 태그)
- `Assets/Prefabs/EnemyBPrefab.prefab` (`Enemy` 태그)
- `Assets/Prefabs/EnemyCPrefab.prefab` (`Enemy` 태그)
- `Assets/Prefabs/EnemyBullet0Prefab.prefab` (`EnemyBullet` 태그)
- `Assets/Prefabs/PlayerBullet0Prefab.prefab` (`PlayerBullet` 태그)
- `Assets/Prefabs/PlayerBullet1Prefab.prefab` (`PlayerBullet` 태그)
- `Assets/Prefabs/ItemCoinPrefab.prefab` (`Item` 태그)
- `Assets/Prefabs/ItemPowerPrefab.prefab` (`Item` 태그)
- `Assets/Prefabs/ItemBoomPrefab.prefab` (`Item` 태그)
- `Assets/Prefabs/SkillBoomPrefab.prefab`

## 코드 구조

### 핵심 스크립트

- `Assets/Scripts/Player.cs`
  - 입력, 이동, 발사, 피격 처리
  - 아이템 획득 처리 (Coin / Power / Boom)
  - 폭탄 스킬(`CreateSkillBoom`) 실행 및 쿨타임 관리
  - 피격 시 UIManager와 연동해 라이프 감소/리스폰
  - 총알은 `ObjectPoolManager.GetPlayerBullet0/1()`로 풀에서 꺼내 사용

- `Assets/Scripts/Enemy.cs`
  - 적 이동, 피격/사망, 타입별 동작
  - `OnEnable()`에서 체력·이동 상태 초기화 (풀 재사용 대응)
  - C 타입의 2연장 총알 발사 — `ObjectPoolManager.GetEnemyBullet0()` 사용
  - 사망/경계 이탈 시 `ObjectPoolManager.ReleaseEnemy()` 로 풀 반환

- `Assets/Scripts/GameManager.cs`
  - 랜덤 적 생성 — `ObjectPoolManager.GetEnemy~()` 사용
  - 스폰 위치/방향 선택 후 `SetActive(true)` → `StartMove()` 순서 보장
  - 확률 기반 아이템 드랍 (`CreateItem`) — `ObjectPoolManager.GetItem()` 사용

- `Assets/Scripts/ObjectPoolManager.cs`
  - 모든 게임 오브젝트의 풀 관리 (싱글톤)
  - `Awake()`에서 `instance` 등록, `Start()`에서 풀 초기화
  - Get / Release 메서드 제공

- `Assets/Scripts/Item.cs`
  - 아이템 타입 정의 (`Coin`, `Power`, `Boom`)
  - 화면 아래 이탈 시 `ObjectPoolManager.ReleaseItem()` 으로 풀 반환

- `Assets/Scripts/EnemyBullet.cs`
  - 적 총알 이동
  - 화면 이탈 시 `ObjectPoolManager.ReleaseBullet()` 으로 풀 반환

- `Assets/Scripts/PlayerBullet.cs`
  - 플레이어 총알 이동
  - 화면 이탈 시 `ObjectPoolManager.ReleaseBullet()` 으로 풀 반환

- `Assets/Scripts/EnemySpawner.cs`
  - 사이드 스폰 지점의 시작/종료점 기반 이동 방향 계산

- `Assets/Scripts/UIManager.cs`
  - 라이프 UI 감소 (`DecreaseLife`)
  - 폭탄 아이콘 증가/감소 (`IncreaseBoom` / `DecreaseBoom`)
  - 점수 반영 (`AddScore`, `AddScoreByEnemyType`)
  - 게임오버/재시작 처리

- `Assets/Scripts/BackgroundManager.cs`
  - 배경 스프라이트 스크롤 처리 (무한 루프)

- `Assets/Scripts/AreaDrawer.cs`
  - 플레이 경계 계산 및 경계 밖 판정

### 보조/테스트 스크립트

- `Assets/Scripts/DrawArrow.cs`: 디버그 화살표 시각화
- `Assets/Scripts/Grammar.cs`, `Assets/Scripts/Test.cs`, `Assets/Scripts/Test2.cs`, `Assets/Scripts/App.cs`, `Assets/Scripts/Programs.cs`: 실험/학습용 코드

## 인스펙터 설정 체크리스트

- **ObjectPoolManager**
  - 각 프리팹 슬롯에 해당 Prefab 할당 필수

- **Player**
  - `firePoint` 할당
  - `skillBoomPrefab` 할당
  - `respawnDelay` 값 설정

- **Enemy(C)**
  - `firePoints` 배열에 최소 2개 Transform 지정

- **GameManager**
  - `spawnPoints`, `spawners` 등록

- **UIManager**
  - 라이프 아이콘 배열(`images`)
  - 폭탄 아이콘 배열(`booms`) — alpha 0/1로 개수 표현
  - `gameOverPanel`, `retryButton`, `scoreText` 연결

- **AreaDrawer**
  - `topLeft`, `topRight`, `bottomLeft`, `bottomRight` 지정

- **BackgroundManager**
  - `backgrounds` 배열에 배경 Transform 등록
  - `speed` 값 설정

## 알려진 주의사항

- 풀에서 꺼낸 오브젝트는 반드시 `Release~()` 로 반환해야 합니다. `Destroy()` 직접 호출 시 풀 리스트에 파괴된 참조가 남아 `MissingReferenceException`이 발생합니다.
- `EnemyBullet.cs`의 `isMove` 플래그는 현재 이동 여부 제어에 직접 사용되지 않으므로 추후 정리 가능합니다.

## 향후 개선 아이디어

- 난이도 곡선(시간/점수 기반 스폰 주기 조정)
- 적 패턴 다양화(곡선 이동, 탄막 패턴)
- 플레이어 무적 시간/피격 이펙트
- 사운드, 폭발 이펙트, 카메라 쉐이크 추가
- 스테이지/웨이브 시스템 도입
- 풀 동적 확장 (풀 소진 시 자동으로 오브젝트 추가 생성)

## 라이선스

필요 시 팀 정책에 맞게 추가하세요.
