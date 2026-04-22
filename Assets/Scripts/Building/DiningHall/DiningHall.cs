using System.Collections;
using UnityEngine;

public class DiningHall : Building
{
    [Header("DiningHall")]
    [SerializeField] int foodCap;
    [SerializeField] int foodGainPerHit;
    [SerializeField] int heldFood;

    [SerializeField] static float range = 20f;
    public static float Range => range;

    [SerializeField] float fillTime = 2f;

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        if (heldFood + interacting.Count <= foodCap)
        {
            if (CrumbManager.reference.ConsumeCrumbs(foodGainPerHit)) {
                SoundSystem.instance.PlaySound("dining-colliding-loop");
                CrumbManager.reference.SpawnCrumbiePopupDecrease(transform.position, foodGainPerHit);
                yield return StartCoroutine(WaitWithProgress(fillTime, duck.ProgressBar));
                heldFood += foodGainPerHit;
                SoundSystem.instance.StopSound("dining-colliding-loop");
            }
        }
    }

    public override bool checkIfUnlocked()
    {
        if (PublicInfo.reference.farmList.Count >= 12) return true;
        else return false;
    }

    public override void Build()
    {
        base.Build();

        PublicInfo.reference.diningHalls.Add(this);
    }

    public override void StartDeconstruction()
    {
        PublicInfo.reference.diningHalls.Remove(this);
        base.StartDeconstruction();
    }

    public bool HasFood(int amount)
    {
        return heldFood >= amount;
    }

    public void TakeFood(int amount)
    {
        heldFood -= amount;
        SoundSystem.instance.PlaySound("dining-hall-active");
    }

}
