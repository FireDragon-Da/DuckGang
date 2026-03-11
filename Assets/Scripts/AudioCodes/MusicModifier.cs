using UnityEngine;

public class MusicModifier : MonoBehaviour
{

    [SerializeField] AudioSource MainTheme;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainTheme = GetComponent<AudioSource>();
        MainTheme.Play();
        MainTheme.enabled = true;
        MainTheme.pitch = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToFast()
    {
        MainTheme.pitch = 2;
    }

    public void SwitchToPaused()
    {
        MainTheme.pitch = .6f;
    }

    public void SwitchToNeutral()
    {
        MainTheme.pitch = .97f;
    }
}
