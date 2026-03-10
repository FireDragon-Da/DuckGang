using System;
using UnityEngine;

public class DuckStats : MonoBehaviour
{
    [SerializeField] private int happiness = 80;
    [SerializeField] private int hunger = 60;
    [SerializeField] private int energy = 70;
    [SerializeField] private int health = 90;

    public int Happiness => happiness;
    public int Hunger => hunger;
    public int Energy => energy;
    public int Health => health;

    public const int MaxStatValue = 100;

    public void ModifyHappiness(int amount) => happiness = Mathf.Clamp(happiness + amount, 0, MaxStatValue);
    public void ModifyHunger(int amount) => hunger = Mathf.Clamp(hunger + amount, 0, MaxStatValue);
    public void ModifyEnergy(int amount) => energy = Mathf.Clamp(energy + amount, 0, MaxStatValue);
    public void ModifyHealth(int amount) => health = Mathf.Clamp(health + amount, 0, MaxStatValue);

    [SerializeField] float averageLifespan;
    [SerializeField] float lifespanVariance;
    float lifespan;
    float curLife;

    void Start()
    {
        lifespan = averageLifespan + UnityEngine.Random.Range(-lifespanVariance/2,lifespanVariance/2);
    }

    void Update()
    {
        curLife += Time.deltaTime;
        if (curLife >= lifespan)
        {
            Die(DeathReason.OldAge);
        }
    }

    public void Die(DeathReason reason)
    {
        String name = GetComponent<DuckNameGen>().CurrentDuckName;

        DeathEvent newEvent = new();
        newEvent.duckName = name;
        newEvent.reason = reason;

        DuckSocietyManager.reference.recentDeaths.Add(newEvent);

        PublicInfo.reference.duckList.Remove(gameObject);
        Destroy(gameObject);
    }
}
