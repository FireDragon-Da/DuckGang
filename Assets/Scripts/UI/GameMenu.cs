using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
    }

    public void FastForwardGame()
    {
        Time.timeScale = 2;
    }

    public void PressSettings()
    {
        Time.timeScale = 0;
    }
}
