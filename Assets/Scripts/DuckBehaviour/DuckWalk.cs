using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DuckWalk : MonoBehaviour
{

    CircleCollider2D col;
    [SerializeField] SpriteRenderer sprite;
    Rigidbody2D rb;

    [SerializeField] float speed;
    float speedModifier = 1;
    float effectiveSpeed => speed * speedModifier;
    [SerializeField] Vector2 direction;
    [SerializeField] float randomBounceOffset;

    List<StatusEffect> statusEffects = new();
    public IReadOnlyList<StatusEffect> StatusEffects => statusEffects;

    Building interacting;
    public bool Interacting => interacting != null;
    [SerializeField] ProgressBar progressBar;
    public ProgressBar ProgressBar => progressBar;

    Queue<Building> taskQueue = new();

    [Header("'Static'")]
    [SerializeField] StatusEffect loveEffect;
    [SerializeField] float loveChance;

    bool canBeGrabbed = true;
    public bool CanBeGrabbed => canBeGrabbed;
    public bool beingDragged;

    DuckStats stats;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<DuckStats>();
    }

    void Start()
    {
        //TODO remove this it is just for temp testing and should be done elsewhere
        PublicInfo.reference.duckList.Add(gameObject);

        if (direction == Vector2.zero)
        {
            ChangeDirection(UnityEngine.Random.insideUnitCircle.normalized);
        }
        else
        {
            ChangeDirection(direction.normalized);
        }

        InitializeStats();
    }

    void InitializeStats()
    {
        
    }

    void Update()
    {
        if (beingDragged)
        {
            return;
        }

        if (interacting == null)
        {
            MoveForward(effectiveSpeed * Time.deltaTime * UpgradeMeetingManager.reference.SpeedIncrease);
        }

        for (int i = statusEffects.Count-1; i >= 0; i--)
        {
            if (statusEffects[i].TickDown(Time.deltaTime))
            {
                statusEffects[i].Removed();
                statusEffects.RemoveAt(i);
            }
        }
    }

    void MoveForward(float distance)
    {
        for (int i = 0; i < 100; i++) { //100 attempt limit to prevent infinite loop

            //Wall hits
            RaycastHit2D wallHit = Physics2D.CircleCast(transform.position, col.radius, direction, distance, LayerMask.GetMask("Floor"));

            if (wallHit)
            {
                transform.Translate((wallHit.distance-col.radius) * direction);
                distance -= wallHit.distance-col.radius;
                WallBounce(Vector2.Reflect(direction, wallHit.normal));
                continue;
            }

            //Duck hits
            RaycastHit2D[] duckHits = Physics2D.CircleCastAll(transform.position, col.radius, direction, distance, LayerMask.GetMask("Ducks"));

            foreach (RaycastHit2D duckHit in duckHits)
            {
                if (duckHit.collider != col)
                {
                    Vector3 otherPos = duckHit.collider.transform.position;
                    DuckWalk otherDuck = duckHit.collider.GetComponent<DuckWalk>();
                    otherDuck.DuckBounce(this);
                    DuckBounce(otherDuck);

                    duckHit.collider.GetComponent<DuckStats>().modHappinessOnCollision(stats.Happiness);
                    stats.modHappinessOnCollision(duckHit.collider.GetComponent<DuckStats>().Happiness);

                    break;
                }
            }
            
            //No hits
            transform.Translate(direction * distance);
            return;
            
        }
    }

    //Never used anymore
    public void Lure(Vector2 positon)
    {
        ChangeDirection(direction = (positon - (Vector2)transform.position).normalized);
    }

    //For bouncing directly off a duck
    public void DuckBounce(DuckWalk other)
    {
        ChangeDirection((transform.position - other.transform.position).normalized);

        if (!stats.IsBaby && !other.stats.IsBaby) {

            float curLoveChance = loveChance;

            if (MeetingManager.reference.hasRomanticSociety)
            {
                curLoveChance += 0.2f;
            }

            curLoveChance += UpgradeMeetingManager.reference.LoveIncrease;

            if (PublicInfo.reference.AnyNestEmpty() && UnityEngine.Random.Range(0,1) < curLoveChance)
            {
                GainStatusEffect(Instantiate(loveEffect));
            }
        }

        if (MeetingManager.reference.hasBeneficialSocialInteraction)
        {
            if (UnityEngine.Random.value > 0.5f)
            {
                CrumbManager.reference.GainCrumbs(2);

            }
        }

    }

    

    void WallBounce(Vector2 targetDirection)
    {
        float statusChance = 0;
        foreach (StatusEffect cur in statusEffects)
        {
            statusChance += cur.Chance;
        }

        if (statusChance >= 1)
        {
            float ranNum = UnityEngine.Random.Range(0,statusChance);

            //Attempt to activate status effects
            foreach (StatusEffect curEffect in statusEffects)
            {
                ranNum -= curEffect.Chance;
                if (ranNum <= 0)
                {
                    Vector2 effectDir = curEffect.Activate(gameObject);
                    if (effectDir == Vector2.zero)
                    {
                        StandardBounce(targetDirection);
                    }
                    else
                    {
                        ChangeDirection(effectDir);
                    }
                    break;
                }
            }

            for (int i = statusEffects.Count-1; i >= 0; i--)
            {
                if (statusEffects[i].EffectTried())
                {
                    statusEffects[i].Removed();
                    statusEffects.RemoveAt(i);
                }
            }

        }
        else
        {
            float ranNum = UnityEngine.Random.Range(0,1);

            //Attempt to activate status effects
            foreach (StatusEffect curEffect in statusEffects)
            {
                ranNum -= curEffect.Chance;
                if (ranNum < 0)
                {
                    Vector2 effectDir = curEffect.Activate(gameObject);
                    if (effectDir == Vector2.zero)
                    {
                        StandardBounce(targetDirection);
                    }
                    else
                    {
                        ChangeDirection(effectDir);
                    }
                    break;
                }
            }

            //Regular movement with random offset
            if (ranNum >= 0)
            {
                StandardBounce(targetDirection);
            }

            for (int i = statusEffects.Count-1; i >= 0; i--)
            {
                if (statusEffects[i].EffectTried())
                {
                    statusEffects[i].Removed();
                    statusEffects.RemoveAt(i);
                }
            }

        }

    }

    void StandardBounce(Vector2 targetDirection)
    {
        Vector2 newDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomBounceOffset,randomBounceOffset),Vector3.forward) * targetDirection;

        ChangeDirection(newDirection);
    }

    void ChangeDirection(Vector2 newDirection)
    {
        if (newDirection == Vector2.zero)
        {
            direction = Vector2.up;
        }
        else
        {
            direction = newDirection;
        }
        sprite.flipX = direction.x > 0;
    }

    public void ForceChangeDirection(Vector2 newDirection)
    {
        direction = newDirection;
        sprite.flipX = direction.x > 0;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            if (beingDragged) {return;}

            Building curBuilding = collision.GetComponent<Building>();

            if (interacting)
            {
                taskQueue.Enqueue(curBuilding); //Add task to queue
            }
            else
            {
                StartCoroutine(BuildingInteraction(curBuilding)); //Start task
                PublicInfo.reference.duckCollideBuildingTimes++;
            }
        }
    }

    IEnumerator BuildingInteraction(Building curBuilding)
    {
        canBeGrabbed = false;
        interacting = curBuilding;
        interacting.StartInteracting(this);

        if ((!stats.IsBaby && stats.WillWork()) || (curBuilding.GetComponent<Playground>() && curBuilding.Built)) {
            yield return StartCoroutine(curBuilding.BuildingInteract(this));
        }

        interacting.EndInteracting(this);
        interacting = null;
        canBeGrabbed = true;

        if (stats.PendingDie)
        {
            stats.LateDie();
            yield break;
        }

        //decrease happiness on interacting if it's not an obstacle
        if (curBuilding.GetComponent<Obstacle>() == null && curBuilding.GetComponent<Grass>() == null) 
        {
            stats.ModifyHappiness(TuningManager.reference.loseOnWork);
        } 
        else //Hit obstacle
        {
            if (MeetingManager.reference.hasStrongAttitude)
            {
                if (UnityEngine.Random.value > 0.5f)
                {
                    stats.ModifyHappiness(1);
                }
            }
        }

        if (!curBuilding.CanWalkOver())
        {
            if (curBuilding.HasUniqueBounce)
            {
                Vector2 targetBounce = curBuilding.UnqiueBounce(this);
                if (targetBounce != Vector2.zero)
                {
                    ChangeDirection(targetBounce);
                }
                else //If the unique bounce can't be used for some reason
                {
                    WallBounce((transform.position - curBuilding.transform.position).normalized);
                }
                
            }
            else
            {
                WallBounce((transform.position - curBuilding.transform.position).normalized);
            }
        }

        while (taskQueue.Count > 0)
        {
            Building targetBuilding = taskQueue.Dequeue();
            if (!targetBuilding) {continue;}

            if (col.IsTouching(targetBuilding.Col))
            {
                interacting = targetBuilding; //Force interacting to ensure no issues
                StartCoroutine(BuildingInteraction(targetBuilding));
                break;
            }
        }
    }

    public bool TryInteract(Building targetBuilding) //For interacting not directly with building
    {
        if (interacting != null || beingDragged || stats.IsBaby) {return false;}

        canBeGrabbed = false;
        interacting = targetBuilding;
        interacting.StartInteracting(this);
        return true;
    }

    public bool EndInteract(Building targetBuilding)
    {
        if (interacting != targetBuilding) {return false;}

        interacting.EndInteracting(this);
        interacting = null;
        canBeGrabbed = true;
        return true;
    }

    public void GainStatusEffect(StatusEffect newEffect)
    {
        //Avoid duplicates
        foreach (StatusEffect curEffect in statusEffects)
        {
            if (newEffect.GetType() == curEffect.GetType())
            {
                curEffect.DuplicateGained();
                statusEffects.Remove(curEffect);
                break;
            }
        }

        statusEffects.Add(newEffect);
        newEffect.Added(this);
    }

    public void RemoveEffect<T>() where T : StatusEffect
    {
        foreach (StatusEffect curEffect in statusEffects)
        {
            if (curEffect is T)
            {
                curEffect.Removed();
                statusEffects.Remove(curEffect);
                return;
            }
        }
    }

    public void GainSpeedModifier(float amount)
    {
        speedModifier += amount;
    }

    public void Place()
    {
        List<Collider2D> hits = new();
        col.Overlap(hits);

        foreach (Collider2D curHit in hits)
        {
            OnTriggerEnter2D(curHit);
        }
    }

}
