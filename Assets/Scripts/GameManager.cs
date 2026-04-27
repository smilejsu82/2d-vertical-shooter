using System;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //싱글톤으로 만들기 
    public static GameManager instance;
    public Transform[] spawnPoints; //위에서 아래로 내려오는 위치 
    public EnemySpawner[]  spawners;    //사이드 위치

    private float delta = 0;

    private int span = 0;

    private void Awake()
    {
        instance = this;
    }


    void Update()
    {
        delta += Time.deltaTime;
        
        if (delta > span)
        {
            //만들어라 
            CreateEnemy();
            delta = 0;
        
            span = Random.Range(1, 4);  //1, 2, 3
        }
    }

    private void CreateEnemy()
    {
        var enemyType = (Enemy.EnemyType) Random.Range(0, 3);  // 0~2 (A, B, C)
        GameObject enemyGo = null;
        switch (enemyType)
        {
            case Enemy.EnemyType.A:
                enemyGo = ObjectPoolManager.instance.GetEnemyA();
                break;
            case Enemy.EnemyType.B:
                enemyGo = ObjectPoolManager.instance.GetEnemyB();
                break;
            case Enemy.EnemyType.C:
                enemyGo = ObjectPoolManager.instance.GetEnemyC();
                break;
        }

        if (enemyGo == null) return;    // 풀 소진 시 스킵

        var dice = Random.Range(0, 2);  //0 또는 1 
        //만약에 0 이라면 위에서 아래로 내려오는거고 
        //1 이라면 사이드 위치를 잡아야 함

        Vector3 moveDir;
        if (dice == 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            enemyGo.transform.position = spawnPoint.position;  // 1. 위치 먼저
            moveDir = Vector2.down;
        }
        else
        {
            EnemySpawner spawner = spawners[Random.Range(0, spawners.Length)];
            enemyGo.transform.position = spawner.startPoint.position;  // 1. 위치 먼저
            moveDir = spawner.GetDir();
        }

        enemyGo.SetActive(true);                                        // 2. 활성화 (OnEnable 실행)
        enemyGo.GetComponent<Enemy>().StartMove(moveDir);               // 3. 이동 시작
    }

    public void CreateItem(Vector3 tpos)
    {
        // None : 30%  (0 ~ 29)
        // Coin : 30%  (30 ~ 59)
        // Power : 20% (60 ~ 79)
        // Boom  : 20% (80 ~ 99)
        int rand = Random.Range(0, 100);

        int index;
        if (rand < 30)
            return;             // None
        else if (rand < 60)
            index = 0;          // Coin
        else if (rand < 80)
            index = 1;          // Power
        else
            index = 2;          // Boom
        
        Item.ItemType itemType = (Item.ItemType) index;

        GameObject itemGo = ObjectPoolManager.instance.GetItem(itemType);
        if (itemGo == null) return;

        itemGo.transform.position = tpos;
        itemGo.SetActive(true);
    }
}
