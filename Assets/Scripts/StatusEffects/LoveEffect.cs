using UnityEngine;

public class LoveEffect : StatusEffect
{

    public override Vector2 Activate(GameObject target)
    {
        Nest nearest = null;
        float nearestSqrDist = float.PositiveInfinity;

        foreach (Nest cur in PublicInfo.reference.nestList)
        {
            if (Mathf.Pow(cur.transform.position.x - target.transform.position.x, 2) +
                Mathf.Pow(cur.transform.position.y - target.transform.position.y, 2) <
                nearestSqrDist)
            {
                nearest = cur;
                nearestSqrDist = Mathf.Pow(cur.transform.position.x - target.transform.position.x, 2) +
                                 Mathf.Pow(cur.transform.position.y - target.transform.position.y, 2);
            }
        }

        return nearest.transform.position - target.transform.position;
    }

    public override bool EffectTried()
    {
        return false;
    }

}
