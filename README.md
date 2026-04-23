# 2D Vertical Shooter

Unity 2D 기반 세로 스크롤 슈팅 프로젝트입니다.
플레이어는 화면 경계 안에서 이동하며 총알을 발사하고, 적(A/B/C 타입)을 처치해 점수를 올립니다.

## 프로젝트 개요

- 장르: 2D Vertical Shooter
- 엔진: Unity 6
- 핵심 플레이:
  - 플레이어 이동 + 연사
  - 랜덤 적 스폰(상단/사이드)
  - 적 타입별 속도/행동 차이
  - 라이프 감소/리스폰/게임오버
  - 점수 누적 및 재시작

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
3. 씬 `Assets/Scenes/SampleScene.unity`를 엽니다.
4. Play 버튼으로 실행합니다.

## 조작법

- 이동: `Horizontal / Vertical` 축 입력 (기본 WASD/화살표)
- 발사: 마우스 왼쪽 버튼 누르고 있기 (`Input.GetMouseButton(0)`)

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

### 점수

- 적 처치 시 타입별 점수:
  - A: +100
  - B: +200
  - C: +300

### 라이프/게임오버

- 라이프 아이콘(`Image[]`)의 alpha를 0으로 만들어 소모 표현
- 마지막 라이프 소모 시 GameOver 패널 활성화
- Retry 버튼으로 현재 씬 재로드

## 씬/오브젝트 구성 (핵심)

메인 플레이 씬:
- `Assets/Scenes/SampleScene.unity`

주요 오브젝트:
- `Player`
- `GameManager`
- `UIManager`
- `AreaDrawer`

주요 프리팹:
- `Assets/Enemy A.prefab` (`Enemy` 태그)
- `Assets/Enemy B.prefab` (`Enemy` 태그)
- `Assets/Enemy C.prefab` (`Enemy` 태그)
- `Assets/EnemyBulletPrefab.prefab` (`EnemyBullet` 태그)
- `Assets/PlayerBullet0Prefab.prefab` (`PlayerBullet` 태그)
- `Assets/PlayerBullet1Prefab.prefab` (`PlayerBullet` 태그)

## 코드 구조

### 핵심 스크립트

- `Assets/Player.cs`
  - 입력, 이동, 발사, 피격 처리
  - 피격 시 UIManager와 연동해 라이프 감소/리스폰

- `Assets/Enemy.cs`
  - 적 이동, 피격/사망, 타입별 동작
  - C 타입의 2연장 총알 발사 처리

- `Assets/GameManager.cs`
  - 랜덤 적 생성
  - 스폰 위치/방향 선택

- `Assets/EnemySpawner.cs`
  - 사이드 스폰 지점의 시작/종료점 기반 이동 방향 계산

- `Assets/UIManager.cs`
  - 라이프 UI 감소
  - 점수 반영
  - 게임오버/재시작 처리

- `Assets/PlayerBullet.cs`, `Assets/EnemyBullet.cs`
  - 총알 이동 및 화면 이탈 시 파괴

- `Assets/AreaDrawer.cs`
  - 플레이 경계 계산 및 경계 밖 판정

### 보조/테스트 스크립트

- `Assets/DrawArrow.cs`: 디버그 화살표 시각화
- `Assets/Grammar.cs`, `Assets/Test.cs`, `Assets/Test2.cs`, `Assets/App.cs`, `Assets/Programs.cs`: 실험/학습용 코드 성격

## 인스펙터 설정 체크리스트

플레이 전에 아래 연결을 확인하면 문제를 줄일 수 있습니다.

- Player
  - `firePoint`, `sideBulletPrefab`, `centerBulletPrefab` 할당
  - `respawnDelay` 값 설정

- Enemy(C)
  - `bulletPrefab` 할당
  - `firePoints` 배열에 최소 2개 Transform 지정

- GameManager
  - `enemies` 배열에 A/B/C 프리팹 등록
  - `spawnPoints`, `spawners` 등록

- UIManager
  - 라이프 아이콘 배열(`images`)
  - `gameOverPanel`, `retryButton`, `scoreText` 연결

- AreaDrawer
  - `topLeft`, `topRight`, `bottomLeft`, `bottomRight` 지정

## 빌드 설정

- Build Settings에 등록된 씬:
  - `Assets/Scenes/SampleScene.unity`

## 알려진 주의사항

- 프로젝트 최초 커밋 이후에는 Unity 생성 폴더(`Library`, `Temp`, `Logs` 등)가 `.gitignore`로 제외되도록 관리하는 것을 권장합니다.
- `EnemyBullet.cs`의 `isMove` 플래그는 현재 이동 여부 제어에 직접 사용되지 않으므로, 추후 필요 시 정리 가능합니다.

## 향후 개선 아이디어

- 난이도 곡선(시간/점수 기반 스폰 주기 조정)
- 적 패턴 다양화(곡선 이동, 탄막 패턴)
- 플레이어 무적 시간/피격 이펙트
- 사운드, 폭발 이펙트, 카메라 쉐이크 추가
- 스테이지/웨이브 시스템 도입

## 라이선스

필요 시 팀 정책에 맞게 추가하세요.
