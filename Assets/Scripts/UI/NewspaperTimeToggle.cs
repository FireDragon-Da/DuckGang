using UnityEngine;

public class NewspaperTimeToggle : MonoBehaviour
{
    [SerializeField] GameObject newspaper;
    public void pauseOrUnpauseBasedOnNewspaper()
    {
        if (newspaper.activeSelf) TimeManager.reference.AddPause();
        else TimeManager.reference.RemovePause();
    }
}
