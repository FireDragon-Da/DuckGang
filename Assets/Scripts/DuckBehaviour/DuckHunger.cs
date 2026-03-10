using UnityEngine;

public class DuckHunger : MonoBehaviour
{

    [SerializeField] float hungerRate;
    float curHungerTimer;
    [SerializeField] float satietyMax;
    float satiety;
    [SerializeField] float fillPerCrumb;
    [SerializeField] float eatRange;

    void Start()
    {
        curHungerTimer = hungerRate;
        satiety = satietyMax;
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

        satiety += fillPerCrumb;
        if (satiety > satietyMax)
        {
            satiety = satietyMax;
        }
    }

    //This will likely be replaced / moved later
    void Starve()
    {
        GetComponent<DuckStats>().Die(DeathReason.Starvation);
    }

}
