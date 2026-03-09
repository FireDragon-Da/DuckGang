using UnityEngine;

public class Grass : MonoBehaviour
{

    void Start()
    {
        //TODO remove this it is just for temp testing and should be done elsewhere
        PublicInfo.reference.grassList.Add(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            CrumbManager.reference.GainCrumbs(1);
            SoundSystem.instance.PlaySound("grass");
        }
    }
}
