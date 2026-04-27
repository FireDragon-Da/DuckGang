using System;
using System.Collections;
using UnityEngine;

public class Playground : Building
{
    [Header("Playground")]
    [SerializeField] float playTime = 1f;

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        DuckStats hitDuck = duck.gameObject.GetComponent<DuckStats>();
        duck.gameObject.GetComponentInChildren<DuckActionIndicator>().SetAction(DuckActionType.Playground);

        yield return StartCoroutine(WaitWithProgress(playTime, duck.ProgressBar));

        hitDuck.ModifyHappiness(TuningManager.reference.playgroundGainInteract);
        
    }
}
