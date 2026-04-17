using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public static StartMenuUI reference;

    [SerializeField] GameObject startMenuPanel;
    [SerializeField] GameObject settingsPanel;

    bool gameStarted;

    void Awake()
    {
        reference = this;
    }

    void Start()
    {
        ShowStartMenu();
    }

    void ShowStartMenu()
    {
        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(true);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        Time.timeScale = 0;
        gameStarted = false;
    }

    public void OnStartGameButton()
    {
        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(false);
        }

        Time.timeScale = 1;
        gameStarted = true;

        if (GameMenu.reference != null)
        {
            GameMenu.reference.PlayGame();
        }
    }

    public void OnSettingsButton()
    {
        if (settingsPanel != null)
        {
            if (startMenuPanel != null)
            {
                startMenuPanel.SetActive(false);
            }

            settingsPanel.SetActive(true);
        }
    }

    public void OnBackFromSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (!gameStarted)
        {
            if (startMenuPanel != null)
            {
                startMenuPanel.SetActive(true);
            }
        }
        else
        {
            if (TimeManager.reference != null)
            {
                TimeManager.reference.RemovePause();
            }
        }
    }

    public void OnQuitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
