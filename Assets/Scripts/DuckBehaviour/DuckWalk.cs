using System;
using System.Collections.Generic;
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
    [SerializeField] Vector2 direction;
    [SerializeField] float randomBounceOffset;

    List<StatusEffect> statusEffects = new();

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
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
    }

    void Update()
    {
        MoveForward(speed * Time.deltaTime);

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
            RaycastHit2D wallHit = Physics2D.Raycast((Vector2)transform.position, direction, col.radius + distance, LayerMask.GetMask("Floor"));

            if (wallHit)
            {
                transform.Translate((wallHit.distance-col.radius) * direction);
                distance -= wallHit.distance-col.radius;
                WallBounce(Vector2.Reflect(direction, wallHit.normal));
                continue;
            }

            //Duck hits
            RaycastHit2D[] duckHits = Physics2D.RaycastAll((Vector2)transform.position+direction, direction, distance, LayerMask.GetMask("Ducks"));

            foreach (RaycastHit2D duckHit in duckHits)
            {
                if (duckHit.collider != col)
                {
                    Vector3 otherPos = duckHit.collider.transform.position;
                    duckHit.collider.GetComponent<DuckWalk>().DuckBounce(transform.position);
                    DuckBounce(otherPos);
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
    public void DuckBounce(Vector3 other)
    {
        ChangeDirection((transform.position - other).normalized);
    }

    void WallBounce(Vector2 targetDirection)
    {
        float statusChance = 0;
        foreach (StatusEffect cur in statusEffects)
        {
            statusChance += cur.Chance;
        }

        if (statusChance > 1)
        {
            float ranNum = UnityEngine.Random.Range(0,statusChance);

            //Attempt to activate status effects
            foreach (StatusEffect curEffect in statusEffects)
            {
                ranNum -= curEffect.Chance;
                if (ranNum <= 0)
                {
                    curEffect.Activate(gameObject);
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
                if (ranNum <= 0)
                {
                    curEffect.Activate(gameObject);
                    break;
                }
            }

            //Regular movement with random offset
            if (ranNum > 0)
            {
                Vector2 newDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomBounceOffset,randomBounceOffset),Vector3.forward) * targetDirection;
                ChangeDirection(newDirection);
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

    void ChangeDirection(Vector2 newDirection)
    {
        direction = newDirection;
        sprite.flipX = direction.x > 0;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            Building curBuilding = collision.GetComponent<Building>();

            curBuilding.BuildingInteract();

            if (!curBuilding.CanWalkOver())
            {
                if (curBuilding.HasUniqueBounce)
                {
                    //Force direction to unique bounce direction
                    ChangeDirection(curBuilding.UnqiueBounce());
                }
                else
                {
                    WallBounce((transform.position - collision.transform.position).normalized);
                }
            }

        }
    }

    public void GainStatusEffect(StatusEffect newEffect)
    {
        //Avoid duplicates
        foreach (StatusEffect curEffect in statusEffects)
        {
            if (newEffect.GetType() == curEffect.GetType())
            {
                statusEffects.Remove(curEffect);
            }
        }

        statusEffects.Add(newEffect);
    }

}
