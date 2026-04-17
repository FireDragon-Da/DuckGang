using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Building : MonoBehaviour
{
    Collider2D col;
    public Collider2D Col => col;

    [SerializeField] protected int width;
    [SerializeField] protected int height;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteRenderer foundationSpriteRenderer;
    [SerializeField] protected bool walkable;
    [SerializeField] protected QuacxiconSO quacxiconSO;
    [SerializeField] protected string buildingName;
    [SerializeField] private TextBox infoTextBox;

    protected bool continueBehavior; //Flag for passing if Interact should continue
    string textinfo;

    protected List<DuckWalk> interacting = new();
    public List<DuckWalk> Interacting => interacting;

    [Header("Construction")]
    [SerializeField] protected float constructionNeeded;
    protected float constructionCount;
    protected bool built;
    public bool Built => built;
    bool hasFinalBuilder;
    [SerializeField] protected int placeCost;
    [SerializeField] protected int buildCost;
    protected string lastBuilderName;



    protected bool removing;
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

    public void StartInteracting(DuckWalk duck)
    {
        interacting.Add(duck);
    }

    public void EndInteracting(DuckWalk duck)
    {
        interacting.Remove(duck);
    }

    public virtual IEnumerator BuildingInteract(DuckWalk duck)
    {
        continueBehavior = false;
        if (removing)
        {
            if (vfxHandler != null) vfxHandler.PlayEffect(removeHitVFX);

            //Add 1 remove
            yield return StartCoroutine(WaitWithProgress(removeTime, duck.ProgressBar));

            AddRemove();
            if (removeCounter >= removeHitsRequired)
            {
                Remove();
            }

            yield break;
        }

        if (!built && !hasFinalBuilder)
        {
            if (!CrumbManager.reference.ConsumeCrumbs(BuildCost))
            {
                yield break;
            }

            //show the crumbie decrease popup animation
            CrumbManager.reference.SpawnCrumbiePopupDecrease(transform.position, BuildCost);

            SoundSystem.instance.PlaySound("building-process-loop");

            if (vfxHandler != null) vfxHandler.PlayEffect(interactVFX);

            // Capture the last builder's name
            DuckNameGen duckNameGen = duck.GetComponent<DuckNameGen>();
            if (duckNameGen != null)
            {
                lastBuilderName = duckNameGen.CurrentDuckName;
            }

            if (constructionCount >= constructionNeeded - 1)
            {
                hasFinalBuilder = true;
            }

            yield return StartCoroutine(WaitWithProgress(buildTime, duck.ProgressBar));

            AddConstruct();
            SoundSystem.instance.StopSound("building-process-loop");
            if (constructionCount >= constructionNeeded)
            {
                Build();
                SoundSystem.instance.PlaySound("building-finished");
            }

            yield break;
        }
        continueBehavior = true;
    }

    void AddConstruct() //Only does the visuals
    {
        constructionCount++;
        progressBar.ChangeFill(constructionCount/constructionNeeded);
    }

    void AddRemove()
    {
        removeCounter++;

        progressBar.ChangeFill((float)removeCounter/removeHitsRequired);
    }

    protected void BasicBuild()
    {
        built = true;
        progressBar.HideBar();
    }

    public virtual void Build()
    {
        BasicBuild();

        foundationSpriteRenderer.enabled = false;
        spriteRenderer.enabled = true;

        textinfo = quacxiconSO.GetRandomLogFromCategory(buildingName);

        //Debug.Log($"[Building] Build() called. textinfo: '{textinfo}', lastBuilderName: '{lastBuilderName}'");
        //Debug.Log($"[Building] infoTextBox is null: {infoTextBox == null}");

        if (infoTextBox != null)
        {
            string outputMessage = textinfo;
            if (!string.IsNullOrEmpty(lastBuilderName))
            {
                outputMessage = $"<color=green>{lastBuilderName + " " + textinfo}</color>";
            }

            //Debug.Log($"[Building] Attempting to add line to TextBox: '{outputMessage}'");
            infoTextBox.AddLine(outputMessage);
            //Debug.Log($"[Building] Line added to TextBox successfully!");
        }
        else
        {
            //Debug.LogWarning($"[Building] Cannot add message - infoTextBox is NULL! Trying to find TextBox again...");

            infoTextBox = TextBox.reference;
            if (infoTextBox != null)
            {
                //Debug.Log($"[Building] Found TextBox on second try: {infoTextBox.name} (Active: {infoTextBox.gameObject.activeSelf})");
                string outputMessage = textinfo;
                if (!string.IsNullOrEmpty(lastBuilderName))
                {
                    outputMessage = $"<color=green>{lastBuilderName + " " + textinfo}</color>";
                }
                infoTextBox.AddLine(outputMessage);
            }
            else
            {
                Debug.LogError("[Building] Still no TextBox found in scene!");
            }
        }

        PublicInfo.reference.constructionList.Remove(this);
        PublicInfo.reference.curBuildingList.Add(this);

        if (vfxHandler != null) vfxHandler.PlayEffect(buildCompleteVFX);
    }

    public virtual void Remove()
    {
        PublicInfo.reference.constructionList.Remove(this);
        Destroy(gameObject);
    }

    public virtual void StartDeconstruction()
    {
        if (removing) {return;}

        removing = true;
        progressBar.ShowBar();
        progressBar.ChangeFill(0);
        PublicInfo.reference.constructionList.Add(this);
        PublicInfo.reference.curBuildingList.Remove(this);
    }

    public virtual void StartBuild()
    {
        foundationSpriteRenderer.enabled = true;
        spriteRenderer.enabled = false;
        progressBar.ShowBar();
        progressBar.ChangeFill(0);
        PublicInfo.reference.constructionList.Add(this);
    }

    public bool GetSpot(int x, int y)
    {
        return filledSpots[x+y * width];
    }

    public virtual Vector2 UnqiueBounce(DuckWalk target)
    {
        //Debug.LogError("Unique Bounce was used when it shouldn't be");
        return Vector2.up;
    }

    void Update()
    {
        if (!built || removing) {return;}

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

    public virtual void TryStartRemove()
    {
        if (interacting.Count == 0)
        {
            StartDeconstruction();
        }
    }

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        infoTextBox = TextBox.reference;
    }

    public Vector2Int GetBottomLeftTile()
    {
        Vector2 tilePos = transform.position;

        tilePos.x -= width / 2f;
        tilePos.y -= height / 2f;

        return MapManager.reference.TransformPosToTilemapPos(tilePos);
    }

}
