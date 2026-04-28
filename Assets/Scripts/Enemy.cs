using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        None = -1,
        A,
        B,
        C
    }
    private SpriteRenderer sr;

    [SerializeField] private float speedA = 2f;
    [SerializeField] private float speedB = 3f;
    [SerializeField] private float speedC = 1f;
    private float speed = 1f;
    public int health;
    private int _initialHealth;
    public Sprite[] sprites;
    public EnemyType enemyType;
    public Transform[] firePoints;
    private float delta = 0;
    private bool isDead = false;

    public float gap = 0.1f;
    void Awake()
    {
        _initialHealth = health;
    }

    private void OnEnable()
    {
        health = _initialHealth;
        isDead = false;
        delta = 0;
        isMove = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        switch (enemyType)
        {
            case EnemyType.A:
                speed = speedA;
                break;
            case EnemyType.B:
                speed = speedB;
                break;
            case EnemyType.C:
                speed = speedC;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            transform.Translate(this.dir * speed * Time.deltaTime, Space.World);

            if (enemyType == EnemyType.C)
            {
                delta += Time.deltaTime;
                if (delta > 1f)
                {
                    Fire();
                    delta = 0;
                }
            }
        }

        if (AreaDrawer.Instance != null && AreaDrawer.Instance.IsOutOfBounds(transform.position))
            ObjectPoolManager.instance.ReleaseEnemy(gameObject);
    }

    private void Fire()
    {
        var playerGo = GameObject.Find("Player");
        if (playerGo != null && firePoints != null && firePoints.Length >= 2)
        {
            var dir = (playerGo.transform.position - transform.position).normalized;

            SpawnEnemyBullet(firePoints[0].position, dir);
            SpawnEnemyBullet(firePoints[1].position, dir);
        }
    }

    private void SpawnEnemyBullet(Vector3 position, Vector3 dir)
    {
        GameObject bulletGo = ObjectPoolManager.instance.GetEnemyBullet0();
        if (bulletGo == null) return;

        bulletGo.transform.position = position;
        bulletGo.GetComponent<EnemyBullet>().StartMove(dir);
        bulletGo.SetActive(true);
    }

    private void Hit(int damage)
    {
        if (isDead)
        {
            return;
        }

        health -= damage;
        sr.sprite = sprites[1];
        Invoke("ReturnDefaultSprite", 0.1f);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.AddScoreByEnemyType(enemyType);
        }
            
        GameManager.instance.CreateItem(transform.position);

        ObjectPoolManager.instance.ReleaseEnemy(gameObject);
    }

    private Vector3 dir;
    private bool isMove = false;
    public void StartMove(Vector3 dir)
    {
        this.dir = dir;
        
        DrawArrow.ForDebug2D(this.transform.position, dir, 10f, Color.red);
        
        isMove = true;
    }

    private void ReturnDefaultSprite()
    {
        sr.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            PlayerBullet playerBullet = other.gameObject.GetComponent<PlayerBullet>();
            Hit(playerBullet.damage);

            ObjectPoolManager.instance.ReleaseBullet(other.gameObject);
        }
    }
}
