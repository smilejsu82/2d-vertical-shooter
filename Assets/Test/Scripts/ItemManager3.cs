using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager3 : MonoBehaviour
{
    public static ItemManager3 Instance;
    
    public GameObject[] items;

    private void Awake()
    {
        Instance = this;
    }


    public void CreateItem(Vector3 pos)
    {
        //아이템을 만들어줄게 
        var prefab = items[Random.Range(0, items.Length)];
        var go = Instantiate(prefab, pos, Quaternion.identity);
        var item3 = go.GetComponent<Item3>();
        if (item3 != null)
        {
            item3.BeginMove();
        }
    }
}
