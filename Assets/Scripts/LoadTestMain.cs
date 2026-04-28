using System.Collections;
using UnityEngine;
using Newtonsoft.Json;

public class LoadTestMain : MonoBehaviour
{
    // stage_data.json의 point 인덱스에 대응하는 스폰 위치 배열
    public Transform[] spawnPoints;

    void Start()
    {
        // Resources 폴더에서 stage_data.json 로드
        var ta = Resources.Load<TextAsset>("stage_data");

        // JSON 문자열을 SpawnData 배열로 역직렬화
        SpawnData[] arr = JsonConvert.DeserializeObject<SpawnData[]>(ta.text);

        StartCoroutine(SpawnRoutine(arr));
    }

    private IEnumerator SpawnRoutine(SpawnData[] datas)
    {
        foreach (SpawnData data in datas)
        {
            // 각 항목의 delay(초)만큼 대기한 뒤 스폰
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

            // 풀이 소진된 경우 이번 항목 스킵
            if (enemyGo == null) continue;

            // point 인덱스에 해당하는 스폰 위치에 배치
            enemyGo.transform.position = spawnPoints[data.point].position;

            // 위치 설정 후 활성화 (OnEnable 실행)
            enemyGo.SetActive(true);

            // 아래 방향으로 이동 시작
            enemyGo.GetComponent<Enemy>().StartMove(Vector2.down);
        }
    }
}
