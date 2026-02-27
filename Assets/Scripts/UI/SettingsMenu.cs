using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackGame()
    {
        Time.timeScale = 1;
    }
}
