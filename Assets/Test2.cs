using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Test2 : MonoBehaviour
{
    public Button btn;
    public SpriteRenderer sr;
    
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            StartCoroutine(FadeOut());
        });
    }

    IEnumerator FadeOut()
    {
        for (int i = 0; i <= 255; i++)
        {
            var newAlpha = 1 - (i / 255f);
            sr.color = new Color(1, 1, 1, newAlpha);
            i++;
            yield return null;
        }
    }
}
