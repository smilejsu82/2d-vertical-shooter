using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image[] images;
    public GameObject gameOverPanel;
    public Button retryButton;
    public TextMeshProUGUI scoreText;
    private int score;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryButtonClick);
        }

        UpdateScoreText();
    }

    private void OnDestroy()
    {
        if (retryButton != null)
        {
            retryButton.onClick.RemoveListener(OnRetryButtonClick);
        }
    }
    
    void Update()
    {
        
    }

    public bool DecreaseLife()
    {
        int visibleLifeCount = 0;
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] != null && images[i].color.a > 0f)
            {
                visibleLifeCount++;
            }
        }

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

                if (visibleLifeCount == 1 && gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true);
                    return true;
                }

                return false;
            }
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        return true;
    }

    public void HandlePlayerHit(GameObject playerGo, Vector3 respawnPosition, float respawnDelay)
    {
        if (playerGo == null)
        {
            return;
        }

        bool isGameOver = DecreaseLife();

        // 피격 시에는 우선 비활성화하고, 게임오버가 아니면 리스폰 코루틴으로 복귀시킨다.
        playerGo.SetActive(false);

        if (isGameOver)
        {
            return;
        }

        StartCoroutine(RespawnPlayer(playerGo, respawnPosition, respawnDelay));
    }

    private System.Collections.IEnumerator RespawnPlayer(GameObject playerGo, Vector3 respawnPosition, float respawnDelay)
    {
        if (playerGo == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(respawnDelay);

        if (playerGo == null)
        {
            yield break;
        }

        if (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            yield break;
        }

        playerGo.transform.position = respawnPosition;
        playerGo.SetActive(true);
    }

    public void OnRetryButtonClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void AddScoreByEnemyType(Enemy.EnemyType enemyType)
    {
        switch (enemyType)
        {
            case Enemy.EnemyType.A:
                score += 100;
                break;
            case Enemy.EnemyType.B:
                score += 200;
                break;
            case Enemy.EnemyType.C:
                score += 300;
                break;
        }

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString("#,##0");
        }
    }
}
