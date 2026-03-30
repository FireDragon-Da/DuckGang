using System;
using System.Collections;
using UnityEngine;

public class Playground : Building
{
    [Header("Playground")]
    [SerializeField] int happinessIncrease = 10;
    [SerializeField] float playTime = 1f;

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        DuckStats hitDuck = duck.gameObject.GetComponent<DuckStats>();

        yield return StartCoroutine(WaitWithProgress(playTime, duck.ProgressBar));

        hitDuck.ModifyHappiness(happinessIncrease);
        
    }
}
