using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Altar : Building
{
    [Header("Altar")]
    [SerializeField] int productionAmount = 150;
    List<DuckWalk> curWatchers = new();
    [SerializeField] int watchersRequired = 3;
    DuckWalk heldDuck;

    [SerializeField] List<Vector2> spotOffsets;

    [SerializeField] AltarGrabber altarGrabber;

    public override void Build()
    {
        base.Build();

        altarGrabber.gameObject.SetActive(true);
    }

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        if (heldDuck == null)
        {
            heldDuck = duck;
            heldDuck.transform.position = transform.position;
            yield return WaitSacrifice();

            heldDuck.GetComponent<DuckStats>().Die(DeathReason.Disappeared);
            PlayInteractBounce();
            CrumbManager.reference.GainCrumbs(productionAmount);
            CrumbManager.reference.SpawnCrumbiePopupIncrease(transform.position, productionAmount);

            foreach (DuckWalk curDuck in curWatchers)
            {
                curDuck.EndInteract(this);
            }
            curWatchers.Clear();

            Remove();
        }
    }

    IEnumerator WaitSacrifice()
    {
        while (curWatchers.Count < watchersRequired)
        {
            yield return null;
        }
    }

    public void GainWatcher(DuckWalk duck)
    {
        if (duck.TryInteract(this))
        {
            duck.transform.position = transform.position + (Vector3)spotOffsets[curWatchers.Count];
            curWatchers.Add(duck);
        }
    }

    public bool HasVictim()
    {
        return heldDuck != null;
    }

}
