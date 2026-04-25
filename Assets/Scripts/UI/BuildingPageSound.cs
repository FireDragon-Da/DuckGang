using UnityEngine;

public class BuildingPageSound : MonoBehaviour
{
    public void PlayBuildingPageSound()
    {
        SoundSystem.instance.PlaySound("open-building-ui");

        TutorialLines.reference.TryActivate(Tutorials.Building);
    }

    public void PlayBuildingPageCloseSound()
    {
        SoundSystem.instance.PlaySound("close-building-ui");

    }
}
