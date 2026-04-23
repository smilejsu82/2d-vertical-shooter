using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] spawnPoints; //위에서 아래로 내려오는 위치 
    public EnemySpawner[]  spawners;    //사이드 위치 

    private float delta = 0;

    private int span = 0; 
    
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
        GameObject prefab = enemies[Random.Range(0, enemies.Length)];

        var dice = Random.Range(0, 2);  //0 또는 1 
        //만약에 0 이라면 위에서 아래로 내려오는거고 
        //1 이라면 사이드 위치를 잡아야 함 

        GameObject enemyGo = Instantiate(prefab);
        var enemy = enemyGo.GetComponent<Enemy>();
        
        if (dice == 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            enemyGo.transform.position = spawnPoint.position;
            enemy.StartMove(Vector2.down);
        }
        else
        {
            EnemySpawner spawner = spawners[Random.Range(0, spawners.Length)];
            enemyGo.transform.position = spawner.startPoint.position;
            enemy.StartMove(spawner.GetDir());
        }
    }
}
