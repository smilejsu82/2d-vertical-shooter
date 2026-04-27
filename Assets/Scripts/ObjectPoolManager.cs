using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    
    public GameObject playerBullet0Prefab;   //작은거
    public GameObject playerBullet1Prefab;   //큰거 
    public GameObject enemyAPrefab;         //A타입적기 
    public GameObject enemyBPrefab;         //B타입적기
    public GameObject enemyCPrefab;         //C타입적기

    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;

    public GameObject enemyBullet0Prefab;
    
    
    private List<GameObject> playerBullet0List = new List<GameObject>();
    private List<GameObject> playerBullet1List = new List<GameObject>();
    private List<GameObject> enemyAList = new List<GameObject>();
    private List<GameObject> enemyBList = new List<GameObject>();
    private List<GameObject> enemyCList = new List<GameObject>();
    private List<GameObject> itemCoinList = new List<GameObject>();
    private List<GameObject> itemPowerList = new List<GameObject>();
    private List<GameObject> itemBoomList = new List<GameObject>();
    private List<GameObject> enemyBullet0List = new List<GameObject>();
    
    
    
    
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            //A타입 
            GameObject enemyAGo = Instantiate(enemyAPrefab);
            enemyAGo.transform.SetParent(transform);
            enemyAGo.SetActive(false);
            enemyAList.Add(enemyAGo);
            
            //B타입 
            GameObject enemyBGo = Instantiate(enemyBPrefab);
            enemyBGo.transform.SetParent(transform);
            enemyBGo.SetActive(false);
            enemyBList.Add(enemyBGo);
            
            //ItemPower 
            GameObject itemPowerGo = Instantiate(itemPowerPrefab);
            itemPowerGo.transform.SetParent(transform);
            itemPowerGo.SetActive(false);
            itemPowerList.Add(itemPowerGo);
            
            //ItemBoom 		
            GameObject itemBoomGo = Instantiate(itemBoomPrefab);
            itemBoomGo.transform.SetParent(transform);
            itemBoomGo.SetActive(false);
            itemBoomList.Add(itemBoomGo);

        }

        for (int i = 0; i < 20; i++)
        {
            //C타입 
            GameObject enemyCGo = Instantiate(enemyCPrefab);
            enemyCGo.transform.SetParent(transform);
            enemyCGo.SetActive(false);
            enemyCList.Add(enemyCGo);
            
            //아이템 Coin
            GameObject itemCoinGo = Instantiate(itemCoinPrefab);
            itemCoinGo.transform.SetParent(transform);
            itemCoinGo.SetActive(false);
            itemCoinList.Add(itemCoinGo);
        }

        for (int i = 0; i < 20; i++)
        {
            //플레이어 작은 총알 
            GameObject playerBullet0Go = Instantiate(playerBullet0Prefab);
            playerBullet0Go.transform.SetParent(transform);
            playerBullet0Go.SetActive(false);
            playerBullet0List.Add(playerBullet0Go);
            
            //플레이어 큰 총알 
            GameObject playerBullet1Go = Instantiate(playerBullet1Prefab);
            playerBullet1Go.transform.SetParent(transform);
            playerBullet1Go.SetActive(false);
            playerBullet1List.Add(playerBullet1Go);
            
            //적기 총알 
            GameObject enemyBullet0Go = Instantiate(enemyBullet0Prefab);
            enemyBullet0Go.transform.SetParent(transform);
            enemyBullet0Go.SetActive(false);
            enemyBullet0List.Add(enemyBullet0Go);
        }
        
        
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("down!!");
        //     GameObject bullet0 = GetPlayerBullet0();
        //     if (bullet0 != null)
        //     {
        //         bullet0.SetActive(true);
        //     }
        // }
    }

    public GameObject GetPlayerBullet0()
    {
        for (int i = 0; i < playerBullet0List.Count; i++)
        {
            GameObject bullet0 = playerBullet0List[i];
            if (bullet0.activeInHierarchy == false)
            {
                return bullet0;
            }
        }

        return null;
    }

    public GameObject GetPlayerBullet1()
    {
        return playerBullet1List.Find(x => !x.activeInHierarchy);
    }

    public GameObject GetEnemyBullet0()
    {
        return enemyBullet0List.Find(x => !x.activeInHierarchy);
    }

    public GameObject GetEnemyA()
    {
        return enemyAList.Find(x => x.activeInHierarchy == false);
    }

    public GameObject GetEnemyB()
    {
        return enemyBList.Find(x => x.activeInHierarchy == false);
    }

    public GameObject GetEnemyC()
    {
        return enemyCList.Find(x => x.activeInHierarchy == false);
    }

    public GameObject GetItem(Item.ItemType itemType)
    {
        switch (itemType)
        {
            case Item.ItemType.Coin:
                return itemCoinList.Find(x => x.activeInHierarchy == false);
                break;
            
            case Item.ItemType.Power:
                return itemPowerList.Find(x => x.activeInHierarchy == false);
                break;
            
            case Item.ItemType.Boom:
                return itemBoomList.Find(x => x.activeInHierarchy == false);
                break;
        }

        return null;
    }

    public void ReleaseBullet(GameObject bulletGo)
    {
        bulletGo.SetActive(false);
        bulletGo.transform.position = Vector3.zero;
    }

    public void ReleaseEnemy(GameObject enemyGo)
    {
        enemyGo.SetActive(false);
        enemyGo.transform.position = Vector3.zero;
    }

    public void ReleaseItem(GameObject itemGo)
    {
        itemGo.SetActive(false);
        itemGo.transform.position = Vector3.zero;
    }
}
