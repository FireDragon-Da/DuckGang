using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : Building
{
    [Header("Drum")]
    [SerializeField] float minActivateTime;
    [SerializeField] float maxActivateTime;
    float activateTime;
    [SerializeField] StatusEffect effect;

    [SerializeField] Collider2D drumRange;

    protected override void UpdateBehavior()
    {
        base.UpdateBehavior();

        activateTime -= Time.deltaTime;
        if (activateTime <= 0)
        {
            activateTime += Random.Range(minActivateTime,maxActivateTime);
            ForceFarm();
        }
    }

    void ForceFarm()
    {
        List<Collider2D> hits = new();
        drumRange.Overlap(hits);

        PlayInteractBounce();

        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Duck"))
            {
                DuckWalk curDuck = col.GetComponent<DuckWalk>();

                curDuck.GainStatusEffect(Instantiate(effect));
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

        PlayInteractBounce();
        duck.GainStatusEffect(Instantiate(effect));

    }

}
