using System.Collections;
using UnityEngine;

public class Farmland : Building
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

        if (curCropCount > 0)
        {
            yield return StartCoroutine(WaitWithProgress(harvestTime, duck.ProgressBar));
            TakeCrop();
        }
        else
        {
            yield return StartCoroutine(WaitWithProgress(waterTime, duck.ProgressBar));
            WaterCrop();
        }
    }

    void TakeCrop()
    {
        curCropCount--;
        CrumbManager.reference.GainCrumbs(cropCrumbGain);
        CrumbManager.reference.SpawnCrumbiePopupIncrease(transform.position, cropCrumbGain);

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
    }

    void Decay()
    {
        curCropCount = 0;
        growTimer = totalGrowTime;

        Color tempColor = spriteRenderer.color;
        tempColor.a = 0;
        spriteRenderer.color = tempColor;
    }

    //If this is removed, remove the whole thing
    public override void Remove()
    {
        if (!holder.destroyed) {
            holder.Remove();
        }
    }

    //If this is removed, remove the whole thing
    public override void StartDeconstruction()
    {
        holder.StartDeconstruction();
    }

}
