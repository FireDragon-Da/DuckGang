using UnityEngine;

public class Grass : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            CrumbManager.reference.GainCrumbs(1);
        }
    }
}
