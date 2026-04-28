using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform[] spawnPoints; // 위에서 아래로 내려오는 위치 (point 인덱스에 대응)
    public EnemySpawner[] spawners; // 사이드 위치

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // stage_data 로드 후 순서대로 적기 스폰
        DataManager.Instance.LoadData();
        List<SpawnData> datas = DataManager.Instance.GetSpawnDatas();
        StartCoroutine(SpawnRoutine(datas));
    }

    private IEnumerator SpawnRoutine(List<SpawnData> datas)
    {
        foreach (SpawnData data in datas)
        {
            // delay(초)만큼 대기
            yield return new WaitForSeconds(data.delay);

            // enemyType에 맞는 적기를 오브젝트 풀에서 가져오기
            GameObject enemyGo = null;
            switch (data.enemyType)
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

            // 풀 소진 시 스킵
            if (enemyGo == null) continue;

            // isSpawner에 따라 스폰 위치와 이동 방향 결정
            Vector3 moveDir;
            if (data.isSpawner)
            {
                EnemySpawner spawner = spawners[data.point];
                enemyGo.transform.position = spawner.startPoint.position;                               // 1. startPoint에 배치
                moveDir = (spawner.endPoint.position - spawner.startPoint.position).normalized;         // start → end 방향
            }
            else
            {
                enemyGo.transform.position = spawnPoints[data.point].position; // 1. 위치 먼저 (상단)
                moveDir = Vector2.down;
            }

            enemyGo.SetActive(true);                                // 2. 활성화 (OnEnable 실행)
            enemyGo.GetComponent<Enemy>().StartMove(moveDir);       // 3. 이동 시작
        }
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
