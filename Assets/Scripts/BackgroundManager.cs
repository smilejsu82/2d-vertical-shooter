using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Transform[] backgrounds;
    public float speed = 1f;

    private float _bgHeight;
    private float _bottomLimit;

    void Start()
    {
        // 스프라이트 크기로 배경 높이 자동 계산
        var sr = backgrounds[0].GetComponent<SpriteRenderer>();
        if (sr != null)
            _bgHeight = sr.bounds.size.y;

        // 카메라 하단 + 배경 높이 만큼 내려가면 재활용 기준선
        _bottomLimit = Camera.main.transform.position.y
                       - Camera.main.orthographicSize
                       - _bgHeight;
    }

    void Update()
    {
        float move = speed * Time.deltaTime;

        foreach (var bg in backgrounds)
        {
            // 아래로 이동
            bg.Translate(Vector3.down * move);

            // 화면 아래로 완전히 벗어나면 가장 위로 재배치
            if (bg.position.y < _bottomLimit)
            {
                float highestY = GetHighestY();
                bg.position = new Vector3(bg.position.x, highestY + _bgHeight, bg.position.z);
            }
        }
    }

    // backgrounds 중 가장 높은 y 위치 반환
    private float GetHighestY()
    {
        float maxY = float.MinValue;
        foreach (var bg in backgrounds)
            if (bg.position.y > maxY)
                maxY = bg.position.y;
        return maxY;
    }
}
