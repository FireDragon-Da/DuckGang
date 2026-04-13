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
    }

    public void PauseGame()
    {
        if (paused) {return;}
        TimeManager.reference.AddPause();
        //AudioListener.pause = true;
    }

    public void PlayGame()
    {
        if (!paused)
        {
            speed = 1;
        }
        else
        {
            TimeManager.reference.AddPause();
        }

        AudioListener.pause = false;
    }

    public void FastForwardGame()
    {
        speed = 2;
        AudioListener.pause = false;
    }

    //Unused now
    public void PressSettings()
    {
        //AudioListener.pause = true;
    }
}
