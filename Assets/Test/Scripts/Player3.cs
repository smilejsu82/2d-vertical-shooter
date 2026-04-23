using System;
using UnityEngine;

public class Player3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var item3 = other.gameObject.GetComponent<Item3>();
        if (item3 == null)
        {
            return;
        }

        Debug.Log(item3.itemType);
        item3.StopMove();
        Destroy(item3.gameObject);
    }
}
