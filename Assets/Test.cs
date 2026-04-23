using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Test : MonoBehaviour
{
    public Image[] images;
    public GameObject gameOverPanel;
    public Button retryButton;
    public TextMeshProUGUI scoreText;
    private int score;
    
    private void Start()
    {
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryButtonClick);
        }
    }

    private void OnDestroy()
    {
        if (retryButton != null)
        {
            retryButton.onClick.RemoveListener(OnRetryButtonClick);
        }
    }
    
    public void OnDieButtonClick()
    {
        Debug.Log("Die!!!");

        int visibleLifeCount = 0;
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] != null && images[i].color.a > 0f)
            {
                visibleLifeCount++;
            }
        }

        // 목숨 아이콘 중 아직 보이는 것을 뒤에서부터 1개 숨긴다.
        for (int i = images.Length - 1; i >= 0; i--)
        {
            if (images[i] == null)
            {
                continue;
            }

            Color color = images[i].color;
            if (color.a > 0f)
            {
                color.a = 0f;
                images[i].color = color;

                // 죽기 직전 목숨이 1개였다면(이번에 마지막 목숨이 사라지면) 게임오버 처리.
                if (visibleLifeCount == 1 && gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true);
                }

                return;
            }
        }

        // 더 이상 숨길 목숨이 없으면 게임오버 패널을 표시한다.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void OnRetryButtonClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    
    public void OnAddScoreButtonClick()
    {
        score += 100;
        scoreText.text = score.ToString("#,##0");
    }
}
