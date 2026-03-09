using UnityEngine;

public class Nest : Building
{
    bool empty = true;
    [SerializeField] NestEffectApplier effectApplier;

    [SerializeField] float defaultEggTimer;
    float curEggTimer;

    [SerializeField] GameObject duckPrefab;

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

    public bool TryLayEgg()
    {
        if (!empty)
        {
            return false;
        }

        empty = false;
        curEggTimer = defaultEggTimer;

        return true;
    }

    protected override void Update()
    {
        base.Update();

        if (!empty)
        {
            curEggTimer -= Time.deltaTime;

            if (curEggTimer <= 0)
            {
                //TODO Proper duck spawning stuff here
                Instantiate(duckPrefab, new(transform.position.x, transform.position.y), new Quaternion());
                empty = true;
            }
        }
    }

    public override void BuildingInteract(DuckWalk duck)
    {
        base.BuildingInteract(duck);

        if (CheckInLove(duck))
        {
            if (TryLayEgg())
            {
                //TODO Duck Egg Cooldown

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
