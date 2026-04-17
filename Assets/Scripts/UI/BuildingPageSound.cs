using UnityEngine;

public class BuildingPageSound : MonoBehaviour
{
    public void PlayBuildingPageSound()
    {
        SoundSystem.instance.PlaySound("open-building-ui");

    }

    public void PlayBuildingPageCloseSound()
    {
        SoundSystem.instance.PlaySound("close-building-ui");

    }
}
