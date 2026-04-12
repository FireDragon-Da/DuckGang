using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Settings")]
    [SerializeField] private string titleString = "Game Over";
    [SerializeField] private QuacxiconSO quacxiconSO;

    void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (titleText != null)
        {
            titleText.text = titleString;
        }

        if (messageText != null && quacxiconSO != null)
        {
            string endingText = quacxiconSO.GetRandomLogFromCategory("GameOver");
            if (string.IsNullOrEmpty(endingText))
            {
                endingText = "Every ducks are gone...\n\nHere ends the Quack Land\n\nHope you will be better£¬\nIn the next Journey";
            }
            messageText.text = endingText;
        }

        Time.timeScale = 0;
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void OnRestartClicked()
    {
        Time.timeScale = 1;

        if (GameOverManager.reference != null)
        {
            GameOverManager.reference.RestartGame();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    void OnQuitClicked()
    {
        Time.timeScale = 1;

        if (GameOverManager.reference != null)
        {
            GameOverManager.reference.QuitGame();
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
