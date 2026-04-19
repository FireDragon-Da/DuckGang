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
            return;
        }

        gameOverTriggered = false;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        Debug.Log("[GameOverManager] Initialized. gameOverTriggered: " + gameOverTriggered);
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
        if (gameOverTriggered)
        {
            Debug.LogWarning("[GameOverManager] TriggerGameOver called but already triggered!");
            return;
        }

        gameOverTriggered = true;
        Debug.Log("[GameOverManager] Game Over triggered!");

        if (TimeManager.reference != null)
        {
            TimeManager.reference.AddPause();
        }
        else
        {
            Debug.LogError("[GameOverManager] TimeManager.reference is NULL!");
        }

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
        Debug.Log("[GameOverManager] Restarting game...");

        if (TimeManager.reference != null)
        {
            TimeManager.reference.RemovePause();
        }

        gameOverTriggered = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        if (TimeManager.reference != null)
        {
            TimeManager.reference.RemovePause();
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
