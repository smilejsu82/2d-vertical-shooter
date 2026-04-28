using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed = 1f;
    
    void Update()
    {
        //아래로 이동 
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
