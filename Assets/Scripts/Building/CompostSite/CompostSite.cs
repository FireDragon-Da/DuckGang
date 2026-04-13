using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompostSite : Building
{
    [Header("CompostSite")]
    [SerializeField] float defaultDecayTimer;
    float decayTimer;
    [SerializeField] float maxPoopCount;
    int poopCount;

    [SerializeField] float poopTime = 2f;

    public static List<CompostSite> activeSites = new();
    List<Farmlike> boostedFarms = new();

    protected override void UpdateBehavior()
    {
        base.UpdateBehavior();

        if (poopCount <= 0) {return;}

        decayTimer -= Time.deltaTime;
        if (decayTimer <= 0)
        {
            poopCount--;
            if (poopCount > 0)
            {
                decayTimer += defaultDecayTimer;
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

        if (poopCount == 0)
        {
            yield return StartCoroutine(WaitWithProgress(poopTime, duck.ProgressBar));
            poopCount++;
            decayTimer = defaultDecayTimer;

            StartBoostProduction();
        }
        else if (poopCount < maxPoopCount)
        {
            yield return StartCoroutine(WaitWithProgress(poopTime, duck.ProgressBar));
            poopCount++;
        }
    }

    void StartBoostProduction()
    {
        activeSites.Add(this);
    }

    List<Farmlike> GetInRange()
    {
        
    }

    bool IsInRange(Farmlike farmlike)
    {
        
    }

}
