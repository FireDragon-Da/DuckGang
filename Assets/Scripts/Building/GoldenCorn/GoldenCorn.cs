using System.Collections.Generic;
using UnityEngine;

public class GoldenCorn : Building
{

    [SerializeField] float defaultWitherTimer;
    float witherTimer;

    [SerializeField] Collider2D hornRange;

    protected override void UpdateBehavior()
    {
        base.UpdateBehavior();

        witherTimer -= Time.deltaTime;
        if (witherTimer <= 0)
        {
            witherTimer += defaultWitherTimer;
            ForceFarm();
        }
    }

    public override Vector2 UnqiueBounce(DuckWalk target)
    {
        SoundSystem.instance.PlaySound("golden-corn-collide");
        return GetClosestFarmlike(target);
    }

    void ForceFarm()
    {
        SoundSystem.instance.PlaySound("golden-corn-active");
        List<Collider2D> hits = new();
        hornRange.Overlap(hits);

        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Duck"))
            {
                DuckWalk curDuck = col.GetComponent<DuckWalk>();

                curDuck.ForceChangeDirection(GetClosestFarmlike(curDuck));
            }
        }

    }

    Vector2 GetClosestFarmlike(DuckWalk target)
    {
        GameObject nearest = null;
        float nearestSqrDist = float.PositiveInfinity;

        foreach (Farmland cur in PublicInfo.reference.farmList)
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

        foreach (Grass cur in PublicInfo.reference.grassList)
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
