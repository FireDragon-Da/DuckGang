using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
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

    [Header("Text and unlock")]
    [SerializeField] public string buildingName;
    public bool unlocked = false;
    

    private TextBox infoTextBox;

    protected bool continueBehavior; //Flag for passing if Interact should continue
    string textinfo;

    protected List<DuckWalk> interacting = new();
    public List<DuckWalk> Interacting => interacting;

    [Header("Construction")]
    [SerializeField] protected float constructionNeeded;
    float actualConstructionNeeded => constructionNeeded - UpgradeMeetingManager.reference.BuildingDecrease;
    protected float constructionCount;
    protected bool built;
    public bool Built => built;
    int currentlyBuilding;
    bool hasFinalBuilder;
    [SerializeField] protected int placeCost;
    [SerializeField] protected int buildCost;
    protected string lastBuilderName;



    protected bool removing;
    [SerializeField] protected int removeHitsRequired = 2;
    protected int removeCounter;
    [SerializeField] float buildTime = 2f;
    [SerializeField] protected float removeTime = 2f;

    [Header("Other")]

    [SerializeField] protected bool[] filledSpots;

    [SerializeField] bool hasUniqueBounce;

    [Header("Visual Effects")]
    [SerializeField] protected BuildingVFXHandler vfxHandler;

    [SerializeField] protected BuildingVFXSO interactVFX;
    [SerializeField] protected BuildingVFXSO buildCompleteVFX;
    [SerializeField] protected BuildingVFXSO removeHitVFX;

    [SerializeField] protected ProgressBar progressBar;

    [Header("Bounce Effect")]
    [SerializeField] protected bool useInteractBounce = true;
    [SerializeField] protected float bounceDuration = 0.12f;
    [SerializeField] protected Vector3 bounceScale = new Vector3(1.32f, 1.3f, 1.4f);

    protected Vector3 builtVisualOriginalScale;
    protected Vector3 foundationVisualOriginalScale;

    protected Coroutine bounceCoroutine;

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

    public float ConstructionNeeded
    {
        get
        {
            return constructionNeeded;
        }
    }

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return spriteRenderer;
        }
    }

    public SpriteRenderer FoundationSpriteRenderer
    {
        get
        {
            return foundationSpriteRenderer;
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
        PlayInteractBounce();
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
            //CrumbManager.reference.SpawnCrumbiePopupDecrease(transform.position, BuildCost);

            SoundSystem.instance.PlaySound("building-process-loop");
            duck.gameObject.GetComponentInChildren<DuckActionIndicator>().SetAction(DuckActionType.Build);

            if (vfxHandler != null) vfxHandler.PlayEffect(interactVFX);

            // Capture the last builder's name
            DuckNameGen duckNameGen = duck.GetComponent<DuckNameGen>();
            if (duckNameGen != null)
            {
                lastBuilderName = duckNameGen.CurrentDuckName;
            }

            if (constructionCount + currentlyBuilding >= actualConstructionNeeded - 1)
            {
                hasFinalBuilder = true;
            }

            currentlyBuilding++;

            yield return StartCoroutine(WaitWithProgress(buildTime, duck.ProgressBar));

            currentlyBuilding--;

            AddConstruct();
            SoundSystem.instance.StopSound("building-process-loop");
            duck.gameObject.GetComponentInChildren<DuckActionIndicator>().ClearAction();
            if (constructionCount >= actualConstructionNeeded)
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
        progressBar.ChangeFill(constructionCount/actualConstructionNeeded);
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

        textinfo = quacxiconSO.GetSpecificLogFromCategory(buildingName, 0);

        if (PublicInfo.reference.buildingEverBuilt.ContainsKey(buildingName))
        {
            if (!PublicInfo.reference.buildingEverBuilt[buildingName])
            {
                PublicInfo.reference.buildingEverBuilt[buildingName] = true;
                DuckSocietyManager.reference.articles.Add(ArticleCreator.reference.namedArticles[buildingName]);
            }
        }

        //Debug.Log($"[Building] Build() called. textinfo: '{textinfo}', lastBuilderName: '{lastBuilderName}'");
        //Debug.Log($"[Building] infoTextBox is null: {infoTextBox == null}");

        if (infoTextBox != null)
        {
            string outputMessage = textinfo;
            if (!string.IsNullOrEmpty(lastBuilderName))
            {
                outputMessage = $"<color=#473510>{lastBuilderName + " " + textinfo}</color>";
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
                    outputMessage = $"<color=#473510>{lastBuilderName + " " + textinfo}</color>";
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
        if (removing) {
            TryUnStartDeconstruction();
            return;
        }

        removing = true;
        progressBar.ShowBar();
        progressBar.ChangeFill(0);
        PublicInfo.reference.constructionList.Add(this);
        PublicInfo.reference.curBuildingList.Remove(this);
    }

    public virtual void TryUnStartDeconstruction()
    {
        if (removeCounter == 0 && built) {
            UnStartDeconstruction();
        }
    }

    public bool CanRemoveInput => !removing || removeCounter == 0;

    public virtual void UnStartDeconstruction()
    {
        removing = false;
        progressBar.HideBar();
        PublicInfo.reference.constructionList.Remove(this);
        PublicInfo.reference.curBuildingList.Add(this);
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

        if (spriteRenderer != null)
        {
            builtVisualOriginalScale = spriteRenderer.transform.localScale;
        }

        if (foundationSpriteRenderer != null)
        {
            foundationVisualOriginalScale = foundationSpriteRenderer.transform.localScale;
        }
    }

    public Vector2Int GetBottomLeftTile()
    {
        Vector2 tilePos = transform.position;

        tilePos.x -= width / 2f;
        tilePos.y -= height / 2f;

        return MapManager.reference.TransformPosToTilemapPos(tilePos);
    }

    protected Transform GetCurrentVisualTransform()
    {
        if (built && spriteRenderer != null)
        {
            return spriteRenderer.transform;
        }

        if (!built && foundationSpriteRenderer != null)
        {
            return foundationSpriteRenderer.transform;
        }

        return transform;
    }
    protected void PlayInteractBounce()
    {
        if (!useInteractBounce) return;

        Transform visual = GetCurrentVisualTransform();
        if (visual == null) return;

        Vector3 originalScale = GetCurrentVisualOriginalScale();

        if (bounceCoroutine != null)
        {
            StopCoroutine(bounceCoroutine);
        }

        visual.localScale = originalScale;
        bounceCoroutine = StartCoroutine(BounceRoutine(visual, originalScale));
    }

    protected IEnumerator BounceRoutine(Transform target, Vector3 originalScale)
    {
        Vector3 bouncedScale = Vector3.Scale(originalScale, bounceScale);

        float halfDuration = bounceDuration * 0.5f;
        float timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = timer / halfDuration;
            target.localScale = Vector3.Lerp(originalScale, bouncedScale, t);
            yield return null;
        }

        timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = timer / halfDuration;
            target.localScale = Vector3.Lerp(bouncedScale, originalScale, t);
            yield return null;
        }

        target.localScale = originalScale;
        bounceCoroutine = null;
    }

    protected Vector3 GetCurrentVisualOriginalScale()
    {
        if (built && spriteRenderer != null)
        {
            return builtVisualOriginalScale;
        }

        if (!built && foundationSpriteRenderer != null)
        {
            return foundationVisualOriginalScale;
        }

        return Vector3.one;
    }

}
