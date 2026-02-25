using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    SpriteRenderer spriteRenderer;
    [SerializeField] bool walkable;

    [Header("Construction")]
    [SerializeField] float constructionNeeded;
    float constructionCount;
    bool built;
    [SerializeField] int buildCost;
    [SerializeField] int placeCost;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
                built = true;
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

}
