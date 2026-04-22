using System.Collections.Generic;
using UnityEngine;

public class HammerSaw : Building
{

    public override bool checkIfUnlocked()
    {
        if (PublicInfo.reference.curBuildingList.Count >= 5) return true;
        else return false;
    }

    public override Vector2 UnqiueBounce(DuckWalk target)
    {
        return GetClosestConstruction(target);
    }

    Vector2 GetClosestConstruction(DuckWalk target)
    {
        GameObject nearest = null;
        float nearestSqrDist = float.PositiveInfinity;

        foreach (Building cur in PublicInfo.reference.constructionList)
        {
            if (Mathf.Pow(cur.transform.position.x - target.transform.position.x, 2) +
                Mathf.Pow(cur.transform.position.y - target.transform.position.y, 2) <
                nearestSqrDist)
            {
                nearest = cur.gameObject;
                nearestSqrDist = Mathf.Pow(cur.transform.position.x - target.transform.position.x, 2) +
                                 Mathf.Pow(cur.transform.position.y - target.transform.position.y, 2);
            }
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

}
