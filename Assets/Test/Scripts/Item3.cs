using System;
using UnityEngine;
using System.Collections;

public class Item3 : MonoBehaviour
{
    public enum ItemType
    {
        None = -1, Coin, Boom, Power
    }
    
    public ItemType itemType = ItemType.None;

    public float speed = 1f;
    private Transform cachedTransform;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        cachedTransform = transform;
    }

    public void BeginMove()
    {
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(Move());
        }
    }

    public void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
    
    private IEnumerator Move()
    {
        while (this)
        {
            if (!cachedTransform)
            {
                moveCoroutine = null;
                yield break;
            }
        
            cachedTransform.Translate(Vector3.down * speed * Time.deltaTime);
        
            if (cachedTransform.position.y <= -5.5f)
                break;
        
            yield return null;  //다음프레임으로 넘긴다
        }
        
        moveCoroutine = null;
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        StopMove();
    }

    private void OnDestroy()
    {
        moveCoroutine = null;
    }
}
