using UnityEngine;

public class BackgroundGroup : MonoBehaviour
{
    public Transform[] backgrounds;

    private float _bgHeight;
    private float _bottomLimit;

    void Start()
    {
        // 스프라이트 크기로 배경 높이 자동 계산
        var sr = backgrounds[0].GetComponent<SpriteRenderer>();
        if (sr != null)
            _bgHeight = sr.bounds.size.y;

        // 카메라 하단 경계 + 배경 높이 만큼 내려가면 재활용 기준선
        _bottomLimit = Camera.main.transform.position.y
                       - Camera.main.orthographicSize
                       - _bgHeight;
    }

    void Update()
    {
        // 가장 아래 배경이 기준선을 넘으면 맨 위로 재배치
        if (GetLowestY() < _bottomLimit)
            RecycleLowestBackground();
    }

    // 가장 아래쪽 배경을 가장 위로 이동
    private void RecycleLowestBackground()
    {
        Transform lowestBg = backgrounds[0];
        foreach (var bg in backgrounds)
            if (bg.position.y < lowestBg.position.y)
                lowestBg = bg;

        float highestY = GetHighestY();
        lowestBg.position = new Vector3(lowestBg.position.x, highestY + _bgHeight, lowestBg.position.z);
    }

    // backgrounds 중 가장 낮은 y 위치 반환
    private float GetLowestY()
    {
        float minY = float.MaxValue;
        foreach (var bg in backgrounds)
            if (bg.position.y < minY)
                minY = bg.position.y;
        return minY;
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
