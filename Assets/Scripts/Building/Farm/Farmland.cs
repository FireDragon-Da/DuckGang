using System.Collections;
using UnityEngine;

public class Farmland : Building , Farmlike
{
    [Header("Farm")]
    float growTimer;
    [SerializeField] int cropGrowCount;
    int curCropCount;
    [SerializeField] float totalGrowTime;

    [SerializeField] float cropWaterGain;
    [SerializeField] int cropCrumbGain;

    float decayTimer;
    [SerializeField] int cropDecayTime;

    FarmHolder holder;

    [SerializeField] float waterTime = 2f;
    [SerializeField] float harvestTime = 2f;

    int compostBoost;

    public override void StartBuild()
    {
        gameObject.SetActive(false);
        progressBar.HideBar();
        holder = transform.parent.GetComponent<FarmHolder>();

        Color tempColor = spriteRenderer.color;

        tempColor.a = 0;

        spriteRenderer.color = tempColor;
    }

    protected override void UpdateBehavior()
    {
        base.UpdateBehavior();

        if (growTimer > 0)
        {
            CropGrow(Time.deltaTime);
        }
        else
        {
            decayTimer -= Time.deltaTime;
            if (decayTimer <= 0)
            {
                Decay();
            }
        }
    }

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        if (!built) //Doesn't have regular building behavior
        {
            yield break;
        }
        else
        {
            PlayInteractBounce();

            if (curCropCount > 0)
            {
                duck.gameObject.GetComponentInChildren<DuckActionIndicator>().SetAction(DuckActionType.Harvest);
                yield return StartCoroutine(WaitWithProgress(harvestTime, duck.ProgressBar));
                TakeCrop(duck);
            }
            else
            {
                duck.gameObject.GetComponentInChildren<DuckActionIndicator>().SetAction(DuckActionType.Water);
                yield return StartCoroutine(WaitWithProgress(waterTime, duck.ProgressBar));
                duck.gameObject.GetComponentInChildren<DuckActionIndicator>().ClearAction();
                WaterCrop();
            }
        }

    }

    void TakeCrop(DuckWalk duck)
    {
        curCropCount--;

        int gain = cropCrumbGain;
        gain += compostBoost;

        if (MeetingManager.reference.hasSerfdomSystem)
        {
            gain *= 2;
            duck.GetComponent<DuckStats>().ModifyHappiness(-6);
        }

        CrumbManager.reference.GainCrumbs(gain);
        PublicInfo.reference.crumbieGainedFromFarmland += gain;
        CrumbManager.reference.SpawnCrumbiePopupIncrease(transform.position, gain);

        //SFX
        SoundSystem.instance.PlaySound("collide-crop");
        duck.gameObject.GetComponentInChildren<DuckActionIndicator>().ClearAction();


        if (curCropCount <= 0)
        {
            growTimer = totalGrowTime;
        }

        Color tempColor = spriteRenderer.color;

        tempColor.a = Mathf.Clamp((float)curCropCount/cropGrowCount,0f,1f);

        spriteRenderer.color = tempColor;
    }

    void WaterCrop()
    {
        CropGrow(cropWaterGain);

    }

    void CropGrow(float amount)
    {
        growTimer -= amount;
        if (growTimer <= 0)
        {
            FinishGrow();
        }
    }

    public void FinishGrow()
    {
        growTimer = 0;
        curCropCount = cropGrowCount;

        Color tempColor = spriteRenderer.color;

        tempColor.a = 1;

        spriteRenderer.color = tempColor;

        decayTimer = cropDecayTime;
    }

    public override void Build()
    {
        gameObject.SetActive(true);
        base.Build();
        FinishGrow();
        //TODO remove this it is just for temp testing and should be done elsewhere
        PublicInfo.reference.farmList.Add(this);
        //TODO this should be cleaner as its added in base and then removed here. not sure what's a better approach
        PublicInfo.reference.curBuildingList.Remove(this);

        foreach (CompostSite site in PublicInfo.reference.activeSites)
        {
            if (site.IsInRange(this))
            {
                site.AddBoosted(this);
                GainBoost();
            }
        }
    }

    public override void Remove()
    {
        PublicInfo.reference.farmList.Remove(this);
        base.Remove();
    }

    void Decay()
    {
        curCropCount = 0;
        growTimer = totalGrowTime;

        Color tempColor = spriteRenderer.color;
        tempColor.a = 0;
        spriteRenderer.color = tempColor;
    }

    public override void StartDeconstruction() //Farm holder deals with this
    {
    }

    public void GainBoost()
    {
        compostBoost++;
    }

    public void RemoveBoost()
    {
        compostBoost--;
    }

}
