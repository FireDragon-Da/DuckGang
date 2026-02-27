using UnityEngine;

public class Farmland : Building
{
    float growTimer;
    [SerializeField] int cropGrowCount;
    int curCropCount;
    [SerializeField] float totalGrowTime;

    [SerializeField] float cropWaterGain;
    [SerializeField] int cropCrumbGain;

    float decayTimer;
    [SerializeField] int cropDecayTime;

    FarmHolder holder;

    void Start()
    {
        holder = transform.parent.GetComponent<FarmHolder>();
    }

    void Update()
    {
        if (!built)
        {
            return;
        }

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
        base.Build();
        FinishGrow();
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
