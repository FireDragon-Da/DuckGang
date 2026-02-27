using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void FastForwardGame()
    {
        Time.timeScale = 2;
        AudioListener.pause = false;
    }

    public void PressSettings()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }
}
