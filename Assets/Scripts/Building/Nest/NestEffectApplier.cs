using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NestEffectApplier : MonoBehaviour
{
    [SerializeField] StatusEffect effect;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            DuckWalk curDuck = collision.GetComponent<DuckWalk>();
            if (!CheckInLove(curDuck))
            {
                curDuck.GainStatusEffect(Instantiate(effect));
            }
        }
    }

    bool CheckInLove(DuckWalk duck)
    {
        foreach (StatusEffect curEffect in duck.StatusEffects)
        {
            if (curEffect is LoveEffect)
            {
                return true;
            }
        }
        return false;
    }

}
