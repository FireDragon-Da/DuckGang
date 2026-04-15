using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CompostSite : Building
{
    [Header("CompostSite")]
    [SerializeField] float defaultDecayTimer;
    float decayTimer;
    [SerializeField] float maxPoopCount;
    int poopCount;
    bool boosting;

    [SerializeField] float poopTime = 2f;

    List<Farmlike> boostedFarms = new();
    [SerializeField] int range;

    protected override void UpdateBehavior()
    {
        base.UpdateBehavior();

        if (poopCount <= 0) {return;}

        decayTimer -= Time.deltaTime;
        if (boosting && decayTimer <= 0)
        {
            poopCount--;
            if (poopCount > 0)
            {
                decayTimer += defaultDecayTimer;
            }
            else
            {
                StopBoosting();
            }
        }
    }

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        if (poopCount >= maxPoopCount)
        {
            yield break;
        }
        
        yield return StartCoroutine(WaitWithProgress(poopTime, duck.ProgressBar));
        if (poopCount == 0)
        {
            poopCount++;
            decayTimer = defaultDecayTimer;

            StartBoostProduction();
        }
        else
        {
            poopCount++;
        }
    }

    void StartBoostProduction()
    {
        boosting = true;

        PublicInfo.reference.activeSites.Add(this);

        List<Farmlike> nearbyFarmlikes = GetInRange();

        foreach (Farmlike farmlike in nearbyFarmlikes)
        {
            boostedFarms.Add(farmlike);
            farmlike.GainBoost();
        }

    }

    void StopBoosting()
    {
        boosting = false;

        foreach (Farmlike farmlike in boostedFarms)
        {
            farmlike.RemoveBoost();
        }

        boostedFarms.Clear();
        PublicInfo.reference.activeSites.Remove(this);
    }

    List<Farmlike> GetInRange()
    {
        List<Farmlike> output = new();

        int min = -range;
        int maxX = width + range;
        int maxY = height + range;

        Vector2Int bottomLeft = MapManager.reference.TilemapPosToArrayPos(GetBottomLeftTile());

        for (int dx = min; dx < maxX; dx++)
        {
            for (int dy = min; dy < maxY; dy++)
            {
                Vector2Int checkPos = bottomLeft + new Vector2Int(dx, dy);

                if (!MapManager.reference.IsArrayPosValid(checkPos)) {continue;}

                Building building = MapManager.reference.buildingArray[checkPos.x, checkPos.y];
                if (building != null)
                {
                    Farmlike farmlike = building.GetComponent<Farmlike>();

                    if (farmlike != null) {
                        output.Add(farmlike);
                    }
                }
            }
        }

        return output;
    }

    //Assumes farmlike is only 1 tile big
    public bool IsInRange(Building farmlike)
    {
        Vector2Int myBottomLeft = MapManager.reference.TilemapPosToArrayPos(GetBottomLeftTile());
        Vector2Int otherPos = MapManager.reference.TilemapPosToArrayPos(farmlike.GetBottomLeftTile());

        return otherPos.x >= myBottomLeft.x - range && otherPos.x < myBottomLeft.x + width + range
        && otherPos.y >= myBottomLeft.y - range && otherPos.y < myBottomLeft.y + height + range;
    }

    public void AddBoosted(Farmlike farmlike)
    {
        boostedFarms.Add(farmlike);
    }

}
