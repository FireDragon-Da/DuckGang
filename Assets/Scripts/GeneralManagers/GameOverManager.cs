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
        reference = this;
        gameOverTriggered = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (reference == this)
        {
            reference = null;
        }
    }

    void Update()
    {
        if (!gameOverTriggered && PublicInfo.reference != null && PublicInfo.reference.duckList.Count <= 0)
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverTriggered)
        {
            return;
        }

        gameOverTriggered = true;

        if (TimeManager.reference != null)
        {
            TimeManager.reference.AddPause();
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

        MouseCursor.reference.SetSprite(MouseCursor.CursorType.Normal);
    }

    public void RestartGame()
    {
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
