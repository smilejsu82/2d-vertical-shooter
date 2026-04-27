using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        None = -1, Coin, Power, Boom
    }

    public ItemType itemType = ItemType.None;
    
    public float speed = 1f;
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -7.85f)
        {
            ObjectPoolManager.instance.ReleaseItem(gameObject);
        }
    }
}
