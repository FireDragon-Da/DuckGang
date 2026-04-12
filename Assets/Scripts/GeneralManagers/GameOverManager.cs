using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager reference;

    [SerializeField] private QuacxiconSO quacxiconSO;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverMessageText;

    private bool gameOverTriggered;

    void Awake()
    {
        if (reference == null)
        {
            reference = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameOverTriggered = false;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameOverTriggered && PublicInfo.reference != null)
        {
            CheckGameOver();
        }
    }

    void CheckGameOver()
    {
        if (PublicInfo.reference.duckList.Count <= 0)
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverTriggered) return;

        gameOverTriggered = true;
        Time.timeScale = 0;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (gameOverMessageText != null && quacxiconSO != null)
        {
            string endingText = quacxiconSO.GetRandomLogFromCategory("GameOver");
            if (string.IsNullOrEmpty(endingText))
            {
                endingText = "Every ducklings are gone...\nHere ends the Quackland";
            }
            gameOverMessageText.text = endingText;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverTriggered = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
