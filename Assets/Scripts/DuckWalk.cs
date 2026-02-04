using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CircleCollider2D))]
public class DuckWalk : MonoBehaviour
{

    CircleCollider2D col;

    [SerializeField] float speed;
    [SerializeField] Vector2 direction;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        if (direction == Vector2.zero)
        {
            direction = Vector2.up;
        }
        else
        {
            direction.Normalize();
        }
    }

    void Update()
    {
        MoveForward(speed * Time.deltaTime);
    }

    void MoveForward(float distance)
    {
        for (int i = 0; i < 100; i++) { //100 attempt limit to prevent infinite loop

            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position+direction*col.radius, direction, distance, LayerMask.GetMask("Floor"));

            if (hit)
            {
                transform.Translate(hit.distance * direction);
                distance -= hit.distance;
                direction = Vector2.Reflect(direction, hit.normal);
            }
            else
            {
                transform.Translate(direction * distance);
                return;
            }
        }
    }

    public void Lure(Vector2 positon)
    {
        direction = (positon - (Vector2)transform.position).normalized;
    }

}
