using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : Building
{
    [Header("Nest")]
    [SerializeField] NestEffectApplier effectApplier;
    bool empty = true;
    bool nestBusy; //Duck is trying to lay egg

    [SerializeField] float defaultEggTime;

    [SerializeField] float layTime = 2f;

    [SerializeField] int totalUses;
    int timesUsed;

    [SerializeField] List<Sprite> sprites;

    public bool Empty
    {
        get
        {
            return empty;
        }
    }

    public override void Build()
    {
        base.Build();

        PublicInfo.reference.nestList.Add(this);
        effectApplier.gameObject.SetActive(true);
    }

    public override void Remove()
    {
        PublicInfo.reference.nestList.Remove(this);
        base.Remove();
    }

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        if (CheckInLove(duck))
        {
            if ((!nestBusy) && empty)
            {
                nestBusy = true;
                yield return StartCoroutine(WaitWithProgress(layTime, duck.ProgressBar));

                //TODO Duck Egg Cooldown

                nestBusy = false;
                empty = false;
                StartCoroutine(WaitEgg());
                duck.RemoveEffect<LoveEffect>();

                spriteRenderer.sprite = sprites[timesUsed * 2 + 1];
            }
        }

    }

    bool CheckInLove(DuckWalk duck)
    {
        foreach (StatusEffect curEffect in duck.StatusEffects)
        {
            if (curEffect is LoveEffect)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator WaitEgg()
    {
        progressBar.ShowBar();
        progressBar.ChangeFill(0);
        float elapsed = 0f;

        while (elapsed < defaultEggTime)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / defaultEggTime;
            progressBar.ChangeFill(progress);

            yield return null;
        }

        progressBar.HideBar();

        DuckSocietyManager.reference.SpawnDuck(transform.position);
        empty = true;
        timesUsed++;
        if (timesUsed >= totalUses)
        {
            Remove();
        } else
        {
            spriteRenderer.sprite = sprites[timesUsed * 2];
        }
    }

}
