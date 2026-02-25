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
            growTimer -= Time.deltaTime;
        }
    }

    public override void BuildingInteract()
    {
        print("yeag");
        base.BuildingInteract();

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
    }

    void WaterCrop()
    {
        CropGrow(cropWaterGain);
    }

    void CropGrow(float amount)
    {
        growTimer -= cropWaterGain;
        if (growTimer <= 0)
        {
            growTimer = 0;
            curCropCount = cropGrowCount;
        }
    }

}
