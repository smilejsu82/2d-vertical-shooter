using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private bool isMove = false;
    private Vector3 dir;
    public float speed = 1f;
    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if (AreaDrawer.Instance != null && AreaDrawer.Instance.IsOutOfBounds(transform.position))
            ObjectPoolManager.instance.ReleaseBullet(gameObject);
    }

    public void StartMove(Vector3 dir)
    {
        isMove = true;
        this.dir = dir;
    }
}
