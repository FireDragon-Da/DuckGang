using UnityEngine;
using System.Collections;

public class DuckHunger : MonoBehaviour
{
    [SerializeField] float hungerRate;
    float curHungerTimer;
    [SerializeField] float satietyMax;
    float satiety;
    [SerializeField] float fillPerCrumb;
    [SerializeField] float eatRange;

    [SerializeField] private SpriteRenderer duckSprite;
    private Color originalColor;

    public float CurrentSatiety => satiety;
    public float MaxSatiety => satietyMax;

    void Start()
    {
        curHungerTimer = hungerRate;
        satiety = satietyMax;

        if (duckSprite == null)
            duckSprite = GetComponentInChildren<SpriteRenderer>();

        if (duckSprite != null)
            originalColor = duckSprite.color;
    }

    void Update()
    {
        curHungerTimer -= Time.deltaTime;
        if (curHungerTimer <= 0)
        {
            curHungerTimer += hungerRate;
            satiety--;

            if (satiety <= eatRange)
            {
                TryEat();
            }
        }
    }

    public void TryEat()
    {
        DiningHall diningHall = FindHall();
        if (!diningHall)
        {
            return;
        }

        DuckFeedingAnimationManager.reference.SpawnFlyingCrumb(diningHall.gameObject, gameObject);

        float fillThisCrumb = fillPerCrumb;

        if (MeetingManager.reference.hasCrumbieAllocationSystem)
        {
            fillThisCrumb += 5;
        }

        satiety += fillThisCrumb;
        if (satiety > satietyMax)
        {
            satiety = satietyMax;
        }
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(EatFeedbackRoutine());
        }
    }

    /// <summary>
    /// Finds a nearby dining hall with food
    /// </summary>
    /// <returns></returns>
    DiningHall FindHall()
    {
        DiningHall diningHall = null;
        float nearestSqrDist = float.PositiveInfinity;

        foreach (DiningHall curHall in PublicInfo.reference.diningHalls)
        {
            if (!curHall.HasFood(1))
            {
                continue;
            }

            float curDist = Mathf.Pow(curHall.transform.position.x - transform.position.x, 2) +
                            Mathf.Pow(curHall.transform.position.y - transform.position.y, 2);
            if (curDist < nearestSqrDist)
            {
                nearestSqrDist = curDist;
                diningHall = curHall;
            }
        }

        if (nearestSqrDist <= DiningHall.Range*DiningHall.Range)
        {
            return diningHall;
        }
        else
        {
            return null;
        }
    }

    private IEnumerator EatFeedbackRoutine()
    {
        if (duckSprite == null) yield break;

        duckSprite.color = Color.green;
        yield return new WaitForSeconds(0.2f);

        duckSprite.color = originalColor;
    }

}