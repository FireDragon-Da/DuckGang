using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour
{
    [SerializeField] protected int width;
    [SerializeField] protected int height;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected bool walkable;

    [Header("Construction")]
    [SerializeField] protected float constructionNeeded;
    protected float constructionCount;
    protected bool built;
    [SerializeField] protected int buildCost;
    [SerializeField] protected int placeCost;

    void Start()
    {
        Color tempColor = spriteRenderer.color;

        tempColor.a = Mathf.Clamp(constructionCount/constructionNeeded,0,0.8f)+0.2f;

        spriteRenderer.color = tempColor;
    }

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

    public bool CanWalkOver()
    {
        return !built || walkable;
    }

    public virtual void BuildingInteract()
    {
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

        Color tempColor = spriteRenderer.color;

        tempColor.a = Mathf.Clamp(constructionCount/constructionNeeded,0,0.8f)+0.2f;

        spriteRenderer.color = tempColor;
    }

    public virtual void Build()
    {
        built = true;
    }

}
