using UnityEngine;

public class UI_Sounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MakeClickSound();
    }

    void MakeClickSound()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundSystem.instance.PlaySound("click");
        }
    }
}
