using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Building : MonoBehaviour
{
    [SerializeField] protected int width;
    [SerializeField] protected int height;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteRenderer foundationSpriteRenderer;
    [SerializeField] protected bool walkable;

    protected bool continueBehavior; //Flag for passing if Interact should continue

    [Header("Construction")]
    [SerializeField] protected float constructionNeeded;
    protected float constructionCount;
    protected bool built;
    [SerializeField] protected int placeCost;
    [SerializeField] protected int buildCost;
    bool removing;
    [SerializeField] int removeHitsRequired = 2;
    int removeCounter;
    [SerializeField] float buildTime = 2f;
    [SerializeField] float removeTime = 2f;

    [Header("Other")]

    [SerializeField] protected bool[] filledSpots;

    [SerializeField] bool hasUniqueBounce;

    [Header("Visual Effects")]
    [SerializeField] protected BuildingVFXHandler vfxHandler;

    [SerializeField] protected BuildingVFXSO interactVFX;
    [SerializeField] protected BuildingVFXSO buildCompleteVFX;
    [SerializeField] protected BuildingVFXSO removeHitVFX;

    [SerializeField] protected ProgressBar progressBar;

    public int Width
    {
        get
        {
            return width;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
    }

    public int PlaceCost
    {
        get
        {
            return placeCost;
        }
    }

    public int BuildCost
    {
        get
        {
            return buildCost;
        }
    }

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return spriteRenderer;
        }
    }

    public bool HasUniqueBounce
    {
        get
        {
            return hasUniqueBounce;
        }
    }

    public bool CanWalkOver()
    {
        return !built || walkable;
    }

    public virtual IEnumerator BuildingInteract(DuckWalk duck)
    {
        continueBehavior = false;
        if (removing)
        {
            if (vfxHandler != null) vfxHandler.PlayEffect(removeHitVFX);

            //Add 1 remove
            yield return StartCoroutine(WaitWithProgress(removeTime, duck.ProgressBar));

            removeCounter++;
            if (removeCounter >= removeHitsRequired)
            {
                Remove();
            }

            yield break;
        }

        if (!built)
        {
            if (!CrumbManager.reference.ConsumeCrumbs(BuildCost))
            {
                yield break;
            }

            //show the crumbie decrease popup animation
            CrumbManager.reference.SpawnCrumbiePopupDecrease(transform.position, BuildCost);

            if (vfxHandler != null) vfxHandler.PlayEffect(interactVFX);

            //Add 1 build
            yield return StartCoroutine(WaitWithProgress(buildTime, duck.ProgressBar));

            AddConstruct();
            if (constructionCount >= constructionNeeded)
            {
                Build();
            }

            yield break;
        }
        continueBehavior = true;
    }

    void AddConstruct()
    {
        constructionCount++;

        progressBar.ChangeFill(constructionCount/constructionNeeded);
    }

    public virtual void Build()
    {
        built = true;
        progressBar.HideBar();

        foundationSpriteRenderer.enabled = false;
        spriteRenderer.enabled = true;

        if (vfxHandler != null) vfxHandler.PlayEffect(buildCompleteVFX);
    }

    public virtual void Remove()
    {
        Destroy(gameObject);
    }

    public virtual void StartDeconstruction()
    {
        removing = true;
    }

    public virtual void StartBuild()
    {
        foundationSpriteRenderer.enabled = true;
        spriteRenderer.enabled = false;
        progressBar.ShowBar();
        progressBar.ChangeFill(0);
    }

    public bool GetSpot(int x, int y)
    {
        return filledSpots[x+y * width];
    }

    public virtual Vector2 UnqiueBounce(DuckWalk target)
    {
        Debug.LogError("Unique Bounce was used when it shouldn't be");
        return Vector2.up;
    }

    void Update()
    {
        if (!built) {return;}

        UpdateBehavior();
    }

    protected virtual void UpdateBehavior()
    {
        
    }

    protected IEnumerator WaitWithProgress(float duration, ProgressBar duckProgressBar)
    {
        duckProgressBar.ShowBar();
        duckProgressBar.ChangeFill(0);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / duration;
            duckProgressBar.ChangeFill(progress);

            yield return null;
        }

        duckProgressBar.HideBar();
    }

}
