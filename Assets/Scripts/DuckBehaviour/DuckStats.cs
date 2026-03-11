using System;
using UnityEngine;

[RequireComponent(typeof(DuckHunger))]
public class DuckStats : MonoBehaviour
{
    private DuckHunger duckHunger;

    [Header("Core Stats")]
    [SerializeField] private int happiness = 80;
    [SerializeField] private int energy = 70;
    [SerializeField] private int health = 90;

    [Header("Lifespan Settings")]
    [SerializeField] float averageLifespan;
    [SerializeField] float lifespanVariance;
    float lifespan;
    float curLife;

    public const int MaxStatValue = 100;

    public int Happiness => happiness;
    public int Energy => energy;
    public int Health => health;

    public int Hunger
    {
        get
        {
            if (duckHunger != null && duckHunger.MaxSatiety > 0)
            {
                return Mathf.RoundToInt((duckHunger.CurrentSatiety / duckHunger.MaxSatiety) * MaxStatValue);
            }
            return 0;
        }
    }

    //max age is around 3000, max in-game age is ~60
    public int Age => Mathf.FloorToInt(curLife / 50);
    public int MaxAge => Mathf.FloorToInt(lifespan);

    private void Awake()
    {
        duckHunger = GetComponent<DuckHunger>();
    }

    void Start()
    {
        lifespan = averageLifespan + UnityEngine.Random.Range(-lifespanVariance / 2, lifespanVariance / 2);

    }

    void Update()
    {
        curLife += Time.deltaTime;
        if (curLife >= lifespan)
        {
            Die(DeathReason.OldAge);
        }
    }

    public void ModifyHappiness(int amount) => happiness = Mathf.Clamp(happiness + amount, 0, MaxStatValue);
    public void ModifyEnergy(int amount) => energy = Mathf.Clamp(energy + amount, 0, MaxStatValue);
    public void ModifyHealth(int amount) => health = Mathf.Clamp(health + amount, 0, MaxStatValue);

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