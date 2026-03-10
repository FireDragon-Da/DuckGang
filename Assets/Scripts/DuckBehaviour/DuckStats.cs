using UnityEngine;

[RequireComponent(typeof(DuckHunger))]
public class DuckStats : MonoBehaviour
{
    private DuckHunger duckHunger;

    [SerializeField] private int happiness = 80;
    [SerializeField] private int energy = 70;
    [SerializeField] private int health = 90;

    [Header("Age Settings")]
    [SerializeField] private int age = 0;
    [SerializeField] private int maxAge = 100;

    public const int MaxStatValue = 100;

    private void Awake()
    {
        duckHunger = GetComponent<DuckHunger>();
    }


    public int Happiness => happiness;
    public int Energy => energy;
    public int Health => health;
    public int Age => age;
    public int MaxAge => maxAge;

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

    public void ModifyHappiness(int amount) => happiness = Mathf.Clamp(happiness + amount, 0, MaxStatValue);
    public void ModifyEnergy(int amount) => energy = Mathf.Clamp(energy + amount, 0, MaxStatValue);
    public void ModifyHealth(int amount) => health = Mathf.Clamp(health + amount, 0, MaxStatValue);
    public void ModifyAge(int amount) => age = Mathf.Clamp(age + amount, 0, maxAge);
}