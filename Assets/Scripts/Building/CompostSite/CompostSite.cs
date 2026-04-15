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
    [SerializeField] float rangeNum;
    [SerializeField] Collider2D range;

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

        List<Collider2D> hits = new();
        range.Overlap(hits);

        foreach (Collider2D col in hits)
        {
            Farmlike farmlike = col.GetComponent<Farmlike>();
            if (farmlike != null)
            {
                output.Add(farmlike);
            }
        }

        return output;
    }

    //Assumes farmlike is only 1 tile big
    public bool IsInRange(Building farmlike)
    {
        if (Mathf.Pow(transform.position.x - farmlike.transform.position.x, 2) +
            Mathf.Pow(transform.position.y - farmlike.transform.position.y, 2) < rangeNum * rangeNum)
        {
            return true;
        }
        return false;
    }

    public void AddBoosted(Farmlike farmlike)
    {
        boostedFarms.Add(farmlike);
    }

}
