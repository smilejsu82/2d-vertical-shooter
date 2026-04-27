using UnityEngine;

/// <summary>
/// 플레이어 총알의 이동 및 수명을 관리합니다.
/// 매 프레임 위쪽으로 이동하며, 화면을 벗어나면 씬에서 제거됩니다.
/// </summary>
public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 총알 이동 속도

    public int damage = 10;
    
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 5.5f)
        {
            ObjectPoolManager.instance.ReleaseBullet(gameObject);
        }
        // if (AreaDrawer.Instance != null && AreaDrawer.Instance.IsOutOfBounds(transform.position))
        //     Destroy(gameObject);
    }
}
