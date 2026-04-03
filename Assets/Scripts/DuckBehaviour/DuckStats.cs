using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

    [Header("Baby Settings")]
    [SerializeField] bool isBaby = true;
    public bool IsBaby => isBaby;
    [SerializeField] float babyDuration = 60f;
    [SerializeField] Animator animator;

    [Header("Happiness")]
    [SerializeField] int minWorkHappiness = -1;

    private bool isDead = false;

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

    public int Age => Mathf.FloorToInt(curLife / 10);
    public int MaxAge => Mathf.FloorToInt(lifespan);

    //passive happiness drop value
    float phd = 0;

    private void Awake()
    {
        duckHunger = GetComponent<DuckHunger>();
    }

    void Start()
    {
        lifespan = averageLifespan + UnityEngine.Random.Range(-lifespanVariance / 2, lifespanVariance / 2);


        StartCoroutine(passiveHappinessDrop());

        if (!isBaby)
        {
            GrowUp();
        }
        else
        {
            curLife = 0;
            animator.SetBool("isBaby", true);
        }

    }

    void Update()
    {
        if (isDead) return;

        curLife += Time.deltaTime;

        if (isBaby && curLife > babyDuration)
        {
            GrowUp();
        }

        if (curLife >= lifespan)
        {
            Die(DeathReason.OldAge);
            Debug.Log("The Duck Dies of Old Age!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
        else if (duckHunger.CurrentSatiety <= 0)
        {
            Die(DeathReason.Starvation);
            Debug.Log("The Duck Dies of Starvation???????????????????????????????????????????????????");
        }
        else if (happiness <= 0)
        {
            Die(DeathReason.Suicide);
            Debug.Log("The Duck Dies of Suicide///////////////////////////////////////////////");
        }
    }

    public void ModifyHappiness(int amount) => happiness = Mathf.Clamp(happiness + amount, 0, MaxStatValue);
    public void ModifyEnergy(int amount) => energy = Mathf.Clamp(energy + amount, 0, MaxStatValue);
    public void ModifyHealth(int amount) => health = Mathf.Clamp(health + amount, 0, MaxStatValue);

    public void Die(DeathReason reason)
    {
        isDead = true;

        if (DuckSocietyManager.reference != null)
        {
            DuckSocietyManager.reference.ProcessDuckDeath(gameObject, reason);
        }
    }

    IEnumerator passiveHappinessDrop()
    {
        yield return new WaitForSeconds(1);
        phd += TuningManager.reference.passiveDrop * (PublicInfo.reference.duckList.Count + 1);

        if (phd > 1)
        {
            ModifyHappiness(-1);
            phd--;
        }
        if (TuningManager.reference.instaKillHappiness) ModifyHappiness(-100);

        StartCoroutine(passiveHappinessDrop());
    }

    public void modHappinessOnCollision(int otherHappiness)
    {
        int happyMod = 0;

        switch (otherHappiness)
        {
            case > 70:
                happyMod = 2;
                break;
            case > 50:
                happyMod = 1;
                break;
            case > 30:
                happyMod = -2;
                break;
            default:
                happyMod = -3;
                break;
        }

        if (MeetingManager.reference.hasCompassionateSociety)
        {
            if (UnityEngine.Random.value > 0.5f)
            {
                happyMod += 2;
            }
        }

        ModifyHappiness(happyMod);
    }

    public void GrowUp()
    {
        isBaby = false;
        curLife = babyDuration;
        animator.SetBool("isBaby", false);
    }

    public void SetHappiness(int amount)
    {
        happiness = amount;
    }

    public bool WillWork()
    {
        return happiness >= minWorkHappiness;
    }

}