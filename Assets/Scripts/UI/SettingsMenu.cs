using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void Activate()
    {
        if (!gameObject.activeSelf)
        {
            TimeManager.reference.AddPause();
        }
        gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackGame()
    {
        if (StartMenuUI.reference != null)
        {
            StartMenuUI.reference.OnBackFromSettings();
        }
        else
        {
            TimeManager.reference.RemovePause();
        }

        gameObject.SetActive(false);
    }
}
