using UnityEngine;

[CreateAssetMenu(fileName = "LoveEffect", menuName = "StatusEffect/LoveEffect")]
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

        if (nearest == null)
        {
            return Vector2.zero;
        }

        Vector2 output = nearest.transform.position - target.transform.position;

        if (output == Vector2.zero)
        {
            return Vector2.up;
        }
        else
        {
            return output.normalized;
        }
    }

    public override void Added(DuckWalk duck)
    {
        base.Added(duck);
        duck.RemoveEffect<NestEffect>();
    }

}
