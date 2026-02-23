using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class DuckWalk : MonoBehaviour
{

    CircleCollider2D col;
    SpriteRenderer sprite;
    Rigidbody2D rb;

    [SerializeField] float speed;
    [SerializeField] Vector2 direction;
    [SerializeField] float randomBounceOffset;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
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

    public void Lure(Vector2 positon)
    {
        ChangeDirection(direction = (positon - (Vector2)transform.position).normalized);
    }

    public void DuckBounce(Vector3 other)
    {
        ChangeDirection((transform.position - other).normalized);
    }

    void WallBounce(Vector2 targetDirection)
    {
        Vector2 newDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomBounceOffset,randomBounceOffset),Vector3.forward) * targetDirection;

        ChangeDirection(newDirection);
    }

    void ChangeDirection(Vector2 newDirection)
    {
        direction = newDirection;
        sprite.flipX = direction.x > 0;
    }

}
