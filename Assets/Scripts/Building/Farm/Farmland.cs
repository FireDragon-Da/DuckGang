using UnityEngine;

public class Farmland : Building
{
    float growTimer;
    [SerializeField] int cropGrowCount;
    int curCropCount;
    [SerializeField] float totalGrowTime;

    [SerializeField] float cropWaterGain;
    [SerializeField] int cropCrumbGain;

    void Update()
    {
        if (growTimer > 0)
        {
            CropGrow(Time.deltaTime);
        }
    }

    public override void BuildingInteract()
    {
        if (!built) //Doesn't have regular building behavior
        {
            return;
        }

        if (curCropCount > 0)
        {
            TakeCrop();
        }
        else
        {
            WaterCrop();
        }
    }

    void TakeCrop()
    {
        curCropCount--;
        CrumbManager.reference.GainCrumbs(cropCrumbGain);

        if (curCropCount <= 0)
        {
            growTimer = totalGrowTime;
        }

        Color tempColor = spriteRenderer.color;

        tempColor.a = Mathf.Clamp((float)curCropCount/cropGrowCount,0f,1f);
        print(tempColor.a);

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
    }

    public override void Build()
    {
        base.Build();
        FinishGrow();
    }

}
