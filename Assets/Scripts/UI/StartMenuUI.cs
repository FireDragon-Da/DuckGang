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

        if (TimeManager.reference != null)
        {
            TimeManager.reference.AddPause();
        }
        gameStarted = false;
    }

    public void OnStartGameButton()
    {
        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(false);
        }

        if (TimeManager.reference != null)
        {
            TimeManager.reference.RemovePause();
        }
        gameStarted = true;
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
