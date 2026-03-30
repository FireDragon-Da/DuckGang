using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AltarGrabber : MonoBehaviour
{

    [SerializeField] Altar altar;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck") && altar.HasVictim())
        {
            DuckWalk curDuck = collision.GetComponent<DuckWalk>();
            
            altar.GainWatcher(curDuck);
        }
    }

}
