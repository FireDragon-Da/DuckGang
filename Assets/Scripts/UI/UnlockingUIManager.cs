using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

//looking for a bunch of code that isn't here anymore? 
//most of it is now being handled either within the Building script, or a new script called ConstructionPageGen,
//and the specific unlock conditions are handled in individual building scripts

public class UnlockingUIManager : MonoBehaviour
{
    public static UnlockingUIManager reference;
    public QuacxiconSO quacxiconSO;

    public List<GameObject> buildingListGO = new List<GameObject>();
    public List<Building> buildingList = new List<Building>();

    /*
    public bool isNestUnlocked;
    public bool isFarmlandUnlocked;
    public bool isGoldenCornUnlocked;
    public bool isPlayGroundUnlocked;
    public bool isCompostsiteUnlocked;
    public bool isSecretSiteUnlocked;
    public bool isHammerSawUnlocked;
    public bool isStrawCraftUnlocked;
    public bool isAltarUnlcoked;
    public bool isDrumUnlocked;
    */
    //private bool hasOpened = false;
    private bool unlockAllForDebug = false;

    public string lockedString;
    /*
    public GameObject NestBuildingBar;
    public GameObject FarmlandBuildingBar;
    public GameObject GoldenCornBuildingBar;
    public GameObject PlaygroundBuildingBar;
    public GameObject CompostsiteBuildingBar;
    public GameObject SecreteSiteBuildingBar;
    public GameObject HammerSawBuildingBar;
    public GameObject StrawCraftBuildingBar;
    public GameObject AltarBuildingBar;
    public GameObject DrumBuildingBar;
    


    private string NestDescription;
    private string FarmlandDescription;
    private string GoldenCornDescription;
    private string PlaygroundDescription;
    private string CompostsiteDescription;
    private string SecreteSiteDescription;
    private string HammerSawDescription;
    private string StrawCraftDescription;
    private string AltarDescription;
    private string DrumDescription;

    private string nestUnlockingText;
    private string farmlandUnlockingText;
    private string goldenCornUnlockingText;
    private string playgroundUnlockingText;
    private string compostsiteUnlockingText;
    private string secreteSiteUnlockingText;
    private string hammerSawUnlockingText;
    private string strawCraftUnlockingText;
    private string altarUnlockingText;
    private string drumUnlockingText;
    */

    private void Awake()
    {
        /*
        NestDescription = quacxiconSO.GetRandomLogFromCategory("Nest");
        FarmlandDescription = quacxiconSO.GetRandomLogFromCategory("Farmland");
        GoldenCornDescription = quacxiconSO.GetRandomLogFromCategory("Golden Corn");
        PlaygroundDescription = quacxiconSO.GetRandomLogFromCategory("Playground");
        CompostsiteDescription = quacxiconSO.GetRandomLogFromCategory("Compost Site");
        SecreteSiteDescription = quacxiconSO.GetRandomLogFromCategory("Secret Site");
        HammerSawDescription = quacxiconSO.GetRandomLogFromCategory("Hammer Saw");
        StrawCraftDescription = quacxiconSO.GetRandomLogFromCategory("Straw Craft");
        AltarDescription = quacxiconSO.GetRandomLogFromCategory("Altar");
        DrumDescription = quacxiconSO.GetRandomLogFromCategory("Drum");
        nestUnlockingText = quacxiconSO.GetRandomLogFromCategory("NestDes");
        farmlandUnlockingText = quacxiconSO.GetRandomLogFromCategory("FarmlandDes");
        goldenCornUnlockingText = quacxiconSO.GetRandomLogFromCategory("Golden CornDes");
        playgroundUnlockingText = quacxiconSO.GetRandomLogFromCategory("PlaygroundDes");
        compostsiteUnlockingText = quacxiconSO.GetRandomLogFromCategory("Compost SiteDes");
        secreteSiteUnlockingText = quacxiconSO.GetRandomLogFromCategory("Secret SiteDes");
        hammerSawUnlockingText = quacxiconSO.GetRandomLogFromCategory("Hammer SawDes");
        strawCraftUnlockingText = quacxiconSO.GetRandomLogFromCategory("Straw CraftDes");
        altarUnlockingText = quacxiconSO.GetRandomLogFromCategory("AltarDes");
        drumUnlockingText = quacxiconSO.GetRandomLogFromCategory("DrumDes");*/

        if (reference == null) reference = this;
        else Destroy(this);

        lockedString = quacxiconSO.GetRandomLogFromCategory("Locked");
    }

    void Start()
    {
        foreach(GameObject g in buildingListGO)
        {
            buildingList.Add(g.GetComponent<Building>());
        }

        if (unlockAllForDebug)
        {
            foreach (Building b in buildingList)
            {
                unlock(b);
            }
        }
    }

    string FormatDescription(Building b)
    {
        int totalCost = (int)(b.ConstructionNeeded * b.BuildCost) + b.PlaceCost;

        return b.Description
            .Replace("[b]", "\n" + (int)b.ConstructionNeeded + " effort needed\n")
            
            .Replace("[c]", totalCost + " crumbs needed");

      //  [b][c]
      // repeated text should also go here
    }



/*
    string FormatUnlockingText(string raw)
    {
        return raw
            .Replace(" (Click", "\n(Click")
            .Replace("Unlocked!", "Unlocked!\n");
    }
    private void OnEnable()
    {
        if (!hasOpened)
        {
            SetAllBuildingsDescriptionLocked();
            hasOpened = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TestIfNestIsUnlocked();
        TestIfFarmlandIsUnlocked();
        TestIfGoldenCornIsUnlocked();
        TestIfCompostSiteIsUnlocked();
        TestIfSecretSiteIsUnlocked();
        TestIfHammerSawIsUnlocked();
        TestIfPlaygroundIsUnlocked();
        TestIfStrawCraftIsUnlocked();
        TestIfAltarIsUnlocked();
        TestIfDrumIsUnlocked();

        if (!hasunlocked)
        {
            ChangeNestDescriptionIfUnlocked();
            ChangeFarmlandDescriptionIfUnlocked();
            ChangeGoldenCornDescriptionIfUnlocked();
            ChangeCompostsiteDescriptionIfUnlocked();
            ChangeSecreteSiteDescriptionIfUnlocked();
            ChangeHammerSawDescriptionIfUnlocked();
            ChangePlaygroundDescriptionIfUnlocked();
            ChangeStrawCraftDescriptionIfUnlocked();
            ChangeAltarDescriptionIfUnlocked();
            ChangeDrumDescriptionIfUnlocked();
            hasunlocked=true;
        }
        
    }
    */
    //this MUST be tied to the open construction button!! if it isn't literally nothing will work!!!!
    public void checkAllUnlocks()
    {
        foreach (Building b in buildingList)
        {
            if (b.checkIfUnlocked()) { unlock(b); }
        }
    }

    void triggerPopup(string txt)
    {
        List<PopupMessageData> popupTextList = new();
        popupTextList.Add(new PopupMessageData(txt));
        PopupManager.Instance.StartPopupSequence(popupTextList);
    }

    //unlock and change description
    void unlock(Building b)
    {
        b.unlocked = true;

        b.buildingBar.GetComponentInChildren<TextMeshProUGUI>().text = FormatDescription(b);

        Button btn = b.buildingBar.GetComponent<Button>();
        btn.interactable = true;

        Transform imageTransform = b.buildingBar.transform.Find("Image");
        imageTransform.gameObject.SetActive(true);

        triggerPopup(b.UnlockText);
    }

    /*
    public void SetAllBuildingsDescriptionLocked()
    {
        
        TextMeshProUGUI nestTmp = NestBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        nestTmp.text = lockedString;

        TextMeshProUGUI farmlandTmp = FarmlandBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        farmlandTmp.text = lockedString;

        TextMeshProUGUI goldenCornTmp = GoldenCornBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        goldenCornTmp.text = lockedString;

        TextMeshProUGUI playgroundTmp = PlaygroundBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        playgroundTmp.text = lockedString;

        TextMeshProUGUI compostTmp = CompostsiteBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        compostTmp.text = lockedString;

        TextMeshProUGUI secretTmp = SecreteSiteBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        secretTmp.text = lockedString;

        TextMeshProUGUI hammerTmp = HammerSawBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        hammerTmp.text = lockedString;

        TextMeshProUGUI strawTmp = StrawCraftBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        strawTmp.text = lockedString;

        TextMeshProUGUI altarTmp = AltarBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        altarTmp.text = lockedString;

        TextMeshProUGUI drumTmp = DrumBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
        drumTmp.text = lockedString;
        
    }*/
    /*
    void TestIfNestIsUnlocked()
    {
        if (!isNestUnlocked && PublicInfo.reference.farmList.Count >= 1)
        {
            isNestUnlocked = true;
            ChangeNestDescriptionIfUnlocked();
        }
        
    }

    void TestIfFarmlandIsUnlocked()
    {
        if (!isFarmlandUnlocked && PublicInfo.reference.crumbieGainedFromGrass >= 10)
        {
            isFarmlandUnlocked = true;
            ChangeFarmlandDescriptionIfUnlocked();
        }

    }

    void TestIfGoldenCornIsUnlocked()
    {
        if (!isGoldenCornUnlocked && PublicInfo.reference.farmList.Count >= 8)
        {
            isGoldenCornUnlocked = true;
            ChangeGoldenCornDescriptionIfUnlocked();
        }
    }

    void TestIfCompostSiteIsUnlocked()
    {
        if (!isCompostsiteUnlocked && PublicInfo.reference.farmList.Count >= 12)
        {
            isCompostsiteUnlocked = true;
            ChangeCompostsiteDescriptionIfUnlocked();
        }
    }

    void TestIfSecretSiteIsUnlocked()
    {
        if (!isSecretSiteUnlocked && PublicInfo.reference.crumbieEverCollected >= 300)
        {
            isSecretSiteUnlocked = true;
            ChangeSecreteSiteDescriptionIfUnlocked();
        }
    }

    void TestIfHammerSawIsUnlocked()
    {
        if (!isHammerSawUnlocked && PublicInfo.reference.curBuildingList.Count >= 3)
        {
            isHammerSawUnlocked = true;
            ChangeHammerSawDescriptionIfUnlocked();
        }
    }

    void TestIfPlaygroundIsUnlocked()
    {
        if (!isPlayGroundUnlocked && PublicInfo.reference.duckList.Count >= 5)
        {
            isPlayGroundUnlocked = true;
            ChangePlaygroundDescriptionIfUnlocked();
        }
    }

    void TestIfStrawCraftIsUnlocked()
    {
        if (!isStrawCraftUnlocked && PublicInfo.reference.crumbieGainedFromFarmland >= 30)
        {
            isStrawCraftUnlocked = true;
            ChangeStrawCraftDescriptionIfUnlocked();
        }
    }

    void TestIfAltarIsUnlocked()
    {
        if (!isAltarUnlcoked && PublicInfo.reference.duckList.Count >= 6)
        {
            isAltarUnlcoked = true;
            ChangeAltarDescriptionIfUnlocked();
        }
    }

    void TestIfDrumIsUnlocked()
    {
        if (!isDrumUnlocked && PublicInfo.reference.duckCollideBuildingTimes >= 70)
        {
            isDrumUnlocked = true;
            ChangeDrumDescriptionIfUnlocked();
        }
    }*/


    /*
    void ChangeNestDescriptionIfUnlocked()
    {
        if (isNestUnlocked)
        {
            TextMeshProUGUI tmp = NestBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(NestDescription);

            Button btn = NestBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = NestBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            //PopupUnlokcingTextManager.instance.ShowText(FormatUnlockingText(nestUnlockingText));

            triggerPopup(nestUnlockingText);
        }
    }

    void ChangeFarmlandDescriptionIfUnlocked()
    {
        if (isFarmlandUnlocked)
        {
            TextMeshProUGUI tmp = FarmlandBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(FarmlandDescription);

            Button btn = FarmlandBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = FarmlandBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(farmlandUnlockingText);
        }
    }

    void ChangeGoldenCornDescriptionIfUnlocked()
    {
        if (isGoldenCornUnlocked)
        {
            TextMeshProUGUI tmp = GoldenCornBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(GoldenCornDescription);

            Button btn = GoldenCornBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = GoldenCornBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(goldenCornUnlockingText);
        }
    }

    void ChangePlaygroundDescriptionIfUnlocked()
    {
        if (isPlayGroundUnlocked)
        {
            TextMeshProUGUI tmp = PlaygroundBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(PlaygroundDescription);

            Button btn = PlaygroundBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = PlaygroundBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(playgroundUnlockingText);
        }
    }

    void ChangeCompostsiteDescriptionIfUnlocked()
    {
        if (isCompostsiteUnlocked)
        {
            TextMeshProUGUI tmp = CompostsiteBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(CompostsiteDescription);

            Button btn = CompostsiteBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = CompostsiteBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(compostsiteUnlockingText);
        }
    }

    void ChangeSecreteSiteDescriptionIfUnlocked()
    {
        if (isSecretSiteUnlocked)
        {
            TextMeshProUGUI tmp = SecreteSiteBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(SecreteSiteDescription);

            Button btn = SecreteSiteBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = SecreteSiteBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(secreteSiteUnlockingText);
        }
    }

    void ChangeHammerSawDescriptionIfUnlocked()
    {
        if (isHammerSawUnlocked)
        {
            TextMeshProUGUI tmp = HammerSawBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(HammerSawDescription);

            Button btn = HammerSawBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = HammerSawBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(hammerSawUnlockingText);
        }
    }

    void ChangeStrawCraftDescriptionIfUnlocked()
    {
        if (isStrawCraftUnlocked)
        {
            TextMeshProUGUI tmp = StrawCraftBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(StrawCraftDescription);

            Button btn = StrawCraftBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = StrawCraftBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(strawCraftUnlockingText);
        }
    }

    void ChangeAltarDescriptionIfUnlocked()
    {
        if (isAltarUnlcoked)
        {
            TextMeshProUGUI tmp = AltarBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(AltarDescription);

            Button btn = AltarBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = AltarBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(altarUnlockingText);
        }
    }

    void ChangeDrumDescriptionIfUnlocked()
    {
        if (isDrumUnlocked)
        {
            TextMeshProUGUI tmp = DrumBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(DrumDescription);

            Button btn = DrumBuildingBar.GetComponent<Button>();
            btn.interactable = true;

            Transform imageTransform = DrumBuildingBar.transform.Find("Image");
            imageTransform.gameObject.SetActive(true);

            triggerPopup(drumUnlockingText);
        }
    }
*/
}
