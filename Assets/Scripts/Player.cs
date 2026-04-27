using System;
using UnityEngine;
using System.Collections;
/// <summary>
/// 플레이어의 입력 처리, 이동 및 총알 발사를 담당합니다.
/// power 값(1~3)에 따라 발사 패턴이 달라집니다.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private Transform firePoint;             // 총알이 생성될 기준 위치
    [SerializeField] private GameObject sideBulletPrefab;    // 좌우 총알 프리팹
    [SerializeField] private GameObject centerBulletPrefab;  // 중앙 강화 총알 프리팹 (power 3 전용)
    [SerializeField] private float moveSpeed = 5f;           // 이동 속도
    [SerializeField] private float fireRate = 0.1f;          // 발사 간격 (초) — 값이 작을수록 빠르게 발사
    [SerializeField] private float sideOffset = 0.25f;       // 좌우 총알의 중앙으로부터 떨어진 거리
    [SerializeField] private float respawnDelay = 2f;        // 피격 후 재등장까지 대기 시간

    public GameObject skillBoomPrefab;

    public int power = 1; // 현재 파워 레벨 (1: 단발, 2: 양옆 2발, 3: 중앙+양옆 3발)
    public int boomCount = 0;   //폭탄개수
    
    private float _fireTimer;       // 마지막 발사 이후 경과 시간
    private Vector2 _spriteExtents; // 스프라이트 절반 크기 (월드 단위)
    private Animator _animator;     // 애니메이션 제어
    private Vector3 _startPosition; // 시작(리스폰) 위치
    private bool _isHitProcessing;  // 연속 충돌 방지 플래그

    // Animator State 파라미터 값 상수
    private const int StateIdle  = 0;
    private const int StateLeft  = 1;
    private const int StateRight = 2;

    void Start()
    {
        _startPosition = transform.position;

        // 스프라이트의 절반 크기를 미리 계산해 경계 계산에 활용
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            _spriteExtents = sr.bounds.extents;

        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();

        if (Input.GetMouseButton(0))
        {
            _fireTimer += Time.deltaTime;

            // fireRate 간격마다 한 번씩 발사
            if (_fireTimer >= fireRate)
            {
                Fire();
                _fireTimer = 0f;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //만약에 skillBoomGo가 null 이라면 (화면에 skillBoomGo가 있다면)
            if (skillBoomGo == null)
            {
                CreateSkillBoom();
            }
            else
            {
                Debug.Log("아직은 폭탄을 사용할수없습니다.");
            }
        }
    }

    private Coroutine skillBoomCoroutine;
    private GameObject skillBoomGo;
    private void CreateSkillBoom()
    {
        if (boomCount <= 0)
        {
            Debug.Log("[SkillBoom] 폭탄이 없어 사용할 수 없습니다.");
            return;
        }

        Debug.Log("SkillBoom 생성!!!");
        
        //skillBoomPrefab 인스턴스 생성 
        skillBoomGo = Instantiate(skillBoomPrefab);
        //2초후에 skillBoomGo를 제거

        if (skillBoomCoroutine != null)
        {
            StopCoroutine(skillBoomCoroutine);
            skillBoomCoroutine = null;
        }
        
        boomCount--;
        UIManager.Instance.DecreaseBoom();
        Debug.Log($"[SkillBoom] 폭탄 사용 | 남은 폭탄: {boomCount}");
        
        //폭탄 사용시 씬에 있는 모든 적기 제거 + 모든 적총알 제거
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        for (int i = 0; i < enemies.Length; i++)
        {
            var enemyGo =  enemies[i];
            var enemy = enemyGo.GetComponent<Enemy>();
            enemy.Die();
        }


        for (int i = 0; i < enemyBullets.Length; i++)
            Destroy(enemyBullets[i]);
                                                              
        

        skillBoomCoroutine = StartCoroutine(WaitForSkillBoom());
    }

    private IEnumerator WaitForSkillBoom()
    {
        yield return new WaitForSeconds(2f);    //2초 후에 아래를 실행함 

        if (skillBoomGo != null)
        {
            Destroy(skillBoomGo);
        }
    }

    /// <summary>
    /// GetAxisRaw 입력을 받아 플레이어를 이동시키고 애니메이션 상태를 전환합니다.
    /// 대각선 이동 시 속도가 빨라지지 않도록 방향 벡터를 정규화합니다.
    /// 이동 후 화면 경계를 벗어나지 않도록 위치를 Clamp합니다.
    /// </summary>
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 대각선 이동 시 단위벡터로 정규화하여 속도를 일정하게 유지
        Vector3 direction = new Vector3(h, v, 0f).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // 화면 경계를 월드 좌표로 변환 (뷰포트 0,0 ~ 1,1)
        Vector3 minBounds = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 maxBounds = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        // 스프라이트 절반 크기만큼 경계를 안쪽으로 좁혀 이미지가 잘리지 않도록 함
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x + _spriteExtents.x, maxBounds.x - _spriteExtents.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y + _spriteExtents.y, maxBounds.y - _spriteExtents.y);
        transform.position = pos;

        // 수평 입력에 따라 애니메이션 State 전환 (좌: 1, 우: 2, 정지: 0)
        if (h < 0f)
            _animator.SetInteger("State", StateLeft);
        else if (h > 0f)
            _animator.SetInteger("State", StateRight);
        else
            _animator.SetInteger("State", StateIdle);
    }

    /// <summary>
    /// 현재 power 레벨에 맞는 발사 패턴을 실행합니다.
    /// </summary>
    private void Fire()
    {
        switch (power)
        {
            case 1: // 중앙 단발
                SpawnBullet(sideBulletPrefab, Vector3.zero);
                break;

            case 2: // 좌우 2발
                sideOffset = 0.1f;
                SpawnBullet(sideBulletPrefab, Vector3.left  * sideOffset);
                SpawnBullet(sideBulletPrefab, Vector3.right * sideOffset);
                break;

            case 3: // 중앙 강화탄 + 좌우 2발
                sideOffset = 0.25f;
                SpawnBullet(centerBulletPrefab, Vector3.zero);
                SpawnBullet(sideBulletPrefab,   Vector3.left  * sideOffset);
                SpawnBullet(sideBulletPrefab,   Vector3.right * sideOffset);
                break;
        }
    }

    /// <summary>
    /// 지정한 프리팹을 firePoint 기준 offset 위치에 생성합니다.
    /// </summary>
    /// <param name="prefab">생성할 총알 프리팹</param>
    /// <param name="offset">firePoint로부터의 로컬 오프셋</param>
    private void SpawnBullet(GameObject prefab, Vector3 offset)
    {
        Instantiate(prefab, firePoint.position + offset, firePoint.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //아이템 관련 
        if (other.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();

            switch (item.itemType)
            {
                case Item.ItemType.Coin:
                    UIManager.Instance.AddScore(1000);
                    Debug.Log("[아이템] Coin 획득 | 스코어 +1000");
                    break;

                case Item.ItemType.Power:
                    int prevPower = power;
                    power = Mathf.Min(power + 1, 3);
                    UIManager.Instance.AddScore(500);
                    Debug.Log($"[아이템] Power 획득 | 스코어 +500 | Power {prevPower} → {power}{(power == 3 ? " (MAX)" : "")}");
                    break;

                case Item.ItemType.Boom:
                    int prevBoom = boomCount;
                    boomCount = Mathf.Min(boomCount + 1, 3);
                    if (boomCount > prevBoom)
                        UIManager.Instance.IncreaseBoom();
                    UIManager.Instance.AddScore(500);
                    Debug.Log($"[아이템] Boom 획득 | 스코어 +500 | 폭탄 {prevBoom} → {boomCount}{(boomCount == 3 ? " (MAX)" : "")}");
                    break;
            }

            Destroy(other.gameObject);
        }

        // 적 본체 또는 적 총알과 충돌 시 라이프 감소 + 일시 비활성화 처리
        if (!other.CompareTag("EnemyBullet") && !other.CompareTag("Enemy"))
        {
            return;
        }

        if (_isHitProcessing)
        {
            return;
        }

        _isHitProcessing = true;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.HandlePlayerHit(gameObject, _startPosition, respawnDelay);
        }
        else
        {
            _isHitProcessing = false;
        }

        Destroy(other.gameObject);
    }

    private void OnEnable()
    {
        _isHitProcessing = false;
    }
}
