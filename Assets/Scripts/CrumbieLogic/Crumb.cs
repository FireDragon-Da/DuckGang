using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Crumb : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            //Potential eating code here
            Destroy(gameObject);
        }
    }
}
