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

    [SerializeField] Sprite fullSprite;
    [SerializeField] Sprite halfSprite;
    [SerializeField] Sprite emptySprite;

    [SerializeField] float fillTime = 2f;

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        if (heldFood + (interacting.Count-1) * foodGainPerHit < foodCap)
        {
            int amountNeeded = (foodGainPerHit + heldFood > foodCap) ? (foodCap - heldFood) : foodGainPerHit;

            if (CrumbManager.reference.ConsumeCrumbs(amountNeeded)) {
                SoundSystem.instance.PlaySound("dining-colliding-loop");
                CrumbManager.reference.SpawnCrumbiePopupDecrease(transform.position, amountNeeded);
                yield return StartCoroutine(WaitWithProgress(fillTime, duck.ProgressBar));
                GainFood(amountNeeded);
                SoundSystem.instance.StopSound("dining-colliding-loop");
            }
        }
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
        PlayInteractBounce();

        UpdateVisual();
    }

    void GainFood(int amount)
    {
        heldFood += amount;

        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (heldFood == foodCap)
        {
            spriteRenderer.sprite = fullSprite;
        }
        else if (heldFood > 0)
        {
            spriteRenderer.sprite = halfSprite;
        }
        else
        {
            spriteRenderer.sprite = emptySprite;
        }
    }

}
