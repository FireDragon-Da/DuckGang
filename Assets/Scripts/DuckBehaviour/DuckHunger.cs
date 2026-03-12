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

            if (satiety <= 0)
            {
                Starve();
            }
        }
    }

    public void TryEat()
    {
        if (!CrumbManager.reference.ConsumeCrumbs(1))
        {
            return;
        }

        DuckFeedingAnimationManager.reference.SpawnFlyingCrumb(this.GetComponent<DuckWalk>());


        satiety += fillPerCrumb;
        if (satiety > satietyMax)
        {
            satiety = satietyMax;
        }
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(EatFeedbackRoutine());
        }
    }

    private IEnumerator EatFeedbackRoutine()
    {
        if (duckSprite == null) yield break;

        duckSprite.color = Color.green;
        yield return new WaitForSeconds(0.2f);

        duckSprite.color = originalColor;
    }

    //This will likely be replaced / moved later
    void Starve()
    {
        GetComponent<DuckStats>().Die(DeathReason.Starvation);
    }

}
