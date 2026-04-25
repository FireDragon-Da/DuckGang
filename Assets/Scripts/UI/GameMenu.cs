using UnityEngine;

// All of the audio listener stuff might need to be redone, doesn't seem right

public class GameMenu : MonoBehaviour
{

    public static GameMenu reference;

    bool paused;
    public bool Paused => paused;
    float speed = 1;
    public float Speed => speed;

    void Awake()
    {
        reference = this;
        paused = false;
        speed = 1;
    }

    void OnDestroy()
    {
        if (reference == this)
        {
            reference = null;
        }
    }

    public void PauseGame()
    {
        if (paused) {return;}
        TimeManager.reference.AddPause();
        paused = true;
        //AudioListener.pause = true;

        TutorialLines.reference.TryActivate(Tutorials.PlayPauseSpeed);
    }

    public void PlayGame()
    {
        if (!paused)
        {
            if (speed == 0)
            {
                speed = 1;
            }
            if (TimeManager.reference != null && TimeManager.reference.GetPauseCount() == 0)
            {
                Time.timeScale = speed;
            }
        }
        else
        {
            TimeManager.reference.RemovePause();
            paused = false;
        }

        AudioListener.pause = false;

        TutorialLines.reference.TryActivate(Tutorials.PlayPauseSpeed);
    }

    public void FastForwardGame()
    {
        speed = 2;
        if (TimeManager.reference != null && TimeManager.reference.GetPauseCount() == 0)
        {
            Time.timeScale = speed;
        }
        AudioListener.pause = false;

        TutorialLines.reference.TryActivate(Tutorials.PlayPauseSpeed);
    }

    public void NormalSpeedGame()
    {
        speed = 1;
        if (TimeManager.reference != null && TimeManager.reference.GetPauseCount() == 0)
        {
            Time.timeScale = speed;
        }
        AudioListener.pause = false;

        TutorialLines.reference.TryActivate(Tutorials.PlayPauseSpeed);
    }

    //Unused now
    public void PressSettings()
    {
        //AudioListener.pause = true;
    }
}
