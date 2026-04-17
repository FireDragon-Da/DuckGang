using UnityEngine;

public class OpenNotePadSound : MonoBehaviour
{

    public void PlayOpenSound()
    {
        SoundSystem.instance.PlaySound("note-open");
    }
}
