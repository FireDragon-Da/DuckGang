using System.Collections;
using UnityEngine;

public class Nest : Building
{
    [Header("Nest")]
    [SerializeField] NestEffectApplier effectApplier;
    bool empty = true;
    bool nestBusy; //Duck is trying to lay egg

    [SerializeField] float defaultEggTimer;
    float curEggTimer;
    [SerializeField] float layTime = 2f;

    [SerializeField] int totalUses;
    int usesLeft;

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

        usesLeft = totalUses;
    }

    public override void Remove()
    {
        PublicInfo.reference.nestList.Remove(this);
        base.Remove();
    }

    protected override void UpdateBehavior()
    {
        base.UpdateBehavior();

        if (!empty)
        {
            curEggTimer -= Time.deltaTime;

            if (curEggTimer <= 0)
            {
                //TODO Proper duck spawning stuff here
                DuckSocietyManager.reference.SpawnDuck(transform.position);
                empty = true;
                usesLeft--;
                if (usesLeft <= 0)
                {
                    Remove();
                }
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

        if (CheckInLove(duck))
        {
            if ((!nestBusy) && empty)
            {
                nestBusy = true;
                yield return StartCoroutine(WaitWithProgress(layTime, duck.ProgressBar));

                //TODO Duck Egg Cooldown

                nestBusy = false;
                empty = false;
                curEggTimer = defaultEggTimer;
                duck.RemoveEffect<LoveEffect>();
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

}
