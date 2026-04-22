using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawCraft : Building
{
    [Header("StrawCraft")]
    [SerializeField] int productionAmount = 5;
    [SerializeField] int totalCapacity = 3;
    int curCapacity;
    [SerializeField] float productionTime = 5f;

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        if (curCapacity < totalCapacity)
        {
            curCapacity++;
            yield return StartCoroutine(WaitWithProgress(productionTime, duck.ProgressBar));
            CrumbManager.reference.GainCrumbs(productionAmount);
            CrumbManager.reference.SpawnCrumbiePopupIncrease(transform.position, productionAmount);
            curCapacity--;
        }
    }

    public override bool checkIfUnlocked()
    {
        if (PublicInfo.reference.crumbieGainedFromFarmland >= 200) return true;
        else return false;
    }

}
