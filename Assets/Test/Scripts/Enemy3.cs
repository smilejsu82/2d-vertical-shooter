using System;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public int hp;
    public int maxHp = 10;

    public Action<Vector3> onDie;
    
    private void Start()
    {
        hp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            onDie(this.transform.position);
            Destroy(gameObject);
        }
    }
}
