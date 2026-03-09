using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Building : MonoBehaviour
{
    [SerializeField] protected int width;
    [SerializeField] protected int height;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteRenderer foundationSpriteRenderer;
    [SerializeField] protected bool walkable;

    [Header("Construction")]
    [SerializeField] protected float constructionNeeded;
    protected float constructionCount;
    protected bool built;
    [SerializeField] protected int placeCost;
    [SerializeField] protected int buildCost;
    bool removing;
    [SerializeField] int removeHitsRequired = 2;
    int removeCounter;

    [SerializeField] protected bool[] filledSpots;

    [SerializeField] bool hasUniqueBounce;

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

    public virtual void BuildingInteract(DuckWalk duck)
    {
        if (removing)
        {
            removeCounter++;
            if (removeCounter >= removeHitsRequired)
            {
                Remove();
            }
            return;
        }

        if (!built)
        {
            if (!CrumbManager.reference.ConsumeCrumbs(BuildCost))
            {
                return;
            }

            AddConstruct();
            if (constructionCount >= constructionNeeded)
            {
                Build();
            }

            return;
        }
    }

    void AddConstruct()
    {
        constructionCount++;

        /*Color tempColor = spriteRenderer.color;

        tempColor.a = Mathf.Clamp(constructionCount/constructionNeeded,0,0.8f)+0.2f;

        spriteRenderer.color = tempColor;*/
    }

    public virtual void Build()
    {
        built = true;

        foundationSpriteRenderer.enabled = false;
        spriteRenderer.enabled = true;
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

    protected virtual void Update()
    {
        if (!built) {return;}
    }

}
