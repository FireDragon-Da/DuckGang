using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(DuckHunger))]
public class DuckStats : MonoBehaviour
{
    private DuckHunger duckHunger;
    [SerializeField] Corpse corpsePrefab;

    [Header("Core Stats")]
    [SerializeField] private int happiness = 80;
    [SerializeField] private int energy = 70;
    [SerializeField] private int health = 90;

    [Header("Lifespan Settings")]
    [SerializeField] float averageLifespan;
    [SerializeField] float lifespanVariance;
    float lifespan;
    float  curLife;

    [Header("Baby Settings")]
    [SerializeField] bool isBaby = true;
    [SerializeField] bool isOld = false;
    public bool IsBaby => isBaby;
    public bool IsOld => isOld;
    [SerializeField] float babyDuration = 60f;
    [SerializeField] Animator animator;

    [Header("Emotional States")]
    [SerializeField] bool isSad = false;
    [SerializeField] bool isHungry = false;
    public bool IsSad => isSad;
    public bool IsHungry => isHungry;
    [SerializeField] int sadThreshold = 30;
    [SerializeField] int hungryThreshold = 30;

    bool wasBothTriggered = false;

    bool isDangling = false;
    public bool IsDangling => isDangling;

    [Header("Happiness")]
    [SerializeField] int minWorkHappiness = -1;

    private bool isDead = false;//Not sure why this is here but I'll leave it

    bool pendingDie;
    public bool PendingDie => pendingDie;
    DeathReason pendingReason;

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
        } else if (isOld)
        {
            GetOld();
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

        if (curLife > lifespan * 0.8f && !isOld)
        {
            GetOld();
        }

        UpdateEmotionalStates();

        if (curLife >= lifespan)
        {
            Die(DeathReason.OldAge);
            //Debug.Log("The Duck Dies of Old Age!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
        else if (duckHunger.CurrentSatiety <= 0)
        {
            Die(DeathReason.Starvation);
            //Debug.Log("The Duck Dies of Starvation???????????????????????????????????????????????????");
        }
        else if (happiness <= 0)
        {
            Die(DeathReason.Suicide);
            //Debug.Log("The Duck Dies of Suicide///////////////////////////////////////////////");
        }
    }

    public void ModifyHappiness(int amount) => happiness = Mathf.Clamp(happiness + amount, 0, MaxStatValue);
    public void ModifyEnergy(int amount) => energy = Mathf.Clamp(energy + amount, 0, MaxStatValue);
    public void ModifyHealth(int amount) => health = Mathf.Clamp(health + amount, 0, MaxStatValue);

    public void LateDie() //Used to force die after working
    {
        Die(pendingReason);
    }

    public void Die(DeathReason reason)
    {
        if (GetComponent<DuckWalk>().Interacting)
        {
            pendingDie = true;
            pendingReason = reason;
            return;
        }

        isDead = true;

        if (DuckSocietyManager.reference != null)
        {
            DuckSocietyManager.reference.ProcessDuckDeath(gameObject, reason);
        }

        Instantiate(corpsePrefab,transform.position,new());
        SoundSystem.instance.PlaySound("duck-dead");
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

    public void GetOld()
    {
        isOld = true;
        curLife = lifespan * 0.8f;
        animator.SetBool("isOld", true);
    }

    void UpdateEmotionalStates()
    {
        bool shouldBeSad = happiness <= sadThreshold;
        bool shouldBeHungry = Hunger <= hungryThreshold;

        if (shouldBeSad && shouldBeHungry)
        {
            if (!wasBothTriggered)
            {
                wasBothTriggered = true;
                if (UnityEngine.Random.value > 0.5f)
                {
                    SetSad(true);
                    SetHungry(false);
                }
                else
                {
                    SetSad(false);
                    SetHungry(true);
                }
            }
        }
        else
        {
            wasBothTriggered = false;

            if (shouldBeSad)
            {
                SetSad(true);
                SetHungry(false);
            }
            else if (shouldBeHungry)
            {
                SetSad(false);
                SetHungry(true);
            }
            else
            {
                SetSad(false);
                SetHungry(false);
            }
        }
    }

    void SetSad(bool value)
    {
        if (isSad != value)
        {
            isSad = value;
            animator.SetBool("isSad", value);
        }
    }

    void SetHungry(bool value)
    {
        if (isHungry != value)
        {
            isHungry = value;
            animator.SetBool("isHungry", value);
        }
    }

    public void SetDangling(bool value)
    {
        if (isDangling == value) return;
        isDangling = value;
        animator.SetBool("isDangling", value);
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