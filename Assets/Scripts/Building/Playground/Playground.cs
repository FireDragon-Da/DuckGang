using System;
using System.Collections;
using UnityEngine;

public class Playground : Building
{

    [SerializeField] int happiness_increase = 10;

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {

        yield return StartCoroutine(base.BuildingInteract(duck));

        if (!built) //Doesn't have regular building behavior
        {
            yield break;
        }

        DuckStats hitDuck = duck.gameObject.GetComponent<DuckStats>();

        hitDuck.ModifyHappiness(happiness_increase);
        
    }
}
