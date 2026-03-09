using UnityEditor;
using UnityEngine;

public class StatusEffect : ScriptableObject
{

    [SerializeField] float chance;
    [SerializeField] float duration;

    public float Chance
    {
        get
        {
            return chance;
        }
    }

    public virtual Vector2 Activate(GameObject target)
    {
        return Vector2.up;
    }

    /// <summary>
    /// Return true if effect should be destroyed
    /// </summary>
    /// <returns></returns>
    public virtual bool EffectTried()
    {
        return false;
    }

    /// <summary>
    /// Return true if effect should be destroyed
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool TickDown(float amount)
    {
        duration -= amount;
        if (duration <= 0)
        {
            return true;
        }
        return false;
    }

    public virtual void Added(DuckWalk duck)
    {
        
    }

    public virtual void Removed()
    {
        Destroy(this);
    }

}
