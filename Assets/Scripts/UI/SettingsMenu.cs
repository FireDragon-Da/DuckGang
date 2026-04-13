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
        TimeManager.reference.RemovePause();
    }
}
