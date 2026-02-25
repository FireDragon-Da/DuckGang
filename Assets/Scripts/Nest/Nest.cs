using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class Nest : MonoBehaviour
{
    public GameObject Duck;
    public float DetectionRadius = 5f;
    [SerializeField] private float spawnCooldown = 1f;

    private CircleCollider2D triggerCollider;
    private float lastSpawnTime = -Mathf.Infinity;
    private HashSet<Collider2D> ducksInRange = new HashSet<Collider2D>();

    private void Awake()
    {
        triggerCollider = GetComponent<CircleCollider2D>();
        triggerCollider.radius = DetectionRadius;
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            ducksInRange.Add(collision);
            AttemptSpawnDuck();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            ducksInRange.Remove(collision);
        }
    }

    private void AttemptSpawnDuck()
    {
        if (Time.time - lastSpawnTime >= spawnCooldown)
        {
            Debug.Log("Duck found in nest area range.");
            Vector3 spawnPosition = transform.position + Vector3.up * 2;
            Instantiate(Duck, spawnPosition, Quaternion.identity);
            lastSpawnTime = Time.time;
        }
    }
}
