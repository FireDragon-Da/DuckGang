using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class UnlockingUIManager : MonoBehaviour
{
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

    public string lockedString;

    public string NestDescription;
    public string FarmlandDescription;
    public string GoldenCornDescription;
    public string PlaygroundDescription;
    public string CompostsiteDescription;
    public string SecreteSiteDescription;
    public string HammerSawDescription;
    public string StrawCraftDescription;
    public string AltarDescription;
    public string DrumDescription;



   

    string FormatDescription(string raw)
    {
        return raw
            .Replace(" Cost:", "\nCost:")
            .Replace(" Unlocking Condition:", "\nUnlocking Condition:");
    }
    private void OnEnable()
    {
        SetAllBuildingsDescriptionLocked();
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
    }

    void SetAllBuildingsDescriptionLocked()
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

    }

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
        if (!isGoldenCornUnlocked && PublicInfo.reference.farmList.Count >= 2)
        {
            isGoldenCornUnlocked = true;
            ChangeGoldenCornDescriptionIfUnlocked();
        }
    }

    void TestIfCompostSiteIsUnlocked()
    {
        if (!isCompostsiteUnlocked && PublicInfo.reference.farmList.Count >= 3)
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
        if (!isHammerSawUnlocked && PublicInfo.reference.constructionList.Count >= 3)
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
        if (!isDrumUnlocked && PublicInfo.reference.duckCollideBuildingTimes >= 20)
        {
            isDrumUnlocked = true;
            ChangeDrumDescriptionIfUnlocked();
        }
    }

    void ChangeNestDescriptionIfUnlocked()
    {
        if (isNestUnlocked)
        {
            TextMeshProUGUI nestTmp = NestBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            nestTmp.text = FormatDescription(NestDescription) ;
        }
    }

    void ChangeFarmlandDescriptionIfUnlocked()
    {
        if (isFarmlandUnlocked)
        {
            TextMeshProUGUI tmp = FarmlandBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = FormatDescription(FarmlandDescription);
        }
    }

    void ChangeGoldenCornDescriptionIfUnlocked()
    {
        if (isGoldenCornUnlocked)
        {
            TextMeshProUGUI tmp = GoldenCornBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = GoldenCornDescription;
        }
    }

    void ChangePlaygroundDescriptionIfUnlocked()
    {
        if (isPlayGroundUnlocked)
        {
            TextMeshProUGUI tmp = PlaygroundBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = PlaygroundDescription;
        }
    }

    void ChangeCompostsiteDescriptionIfUnlocked()
    {
        if (isCompostsiteUnlocked)
        {
            TextMeshProUGUI tmp = CompostsiteBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = CompostsiteDescription;
        }
    }

    void ChangeSecreteSiteDescriptionIfUnlocked()
    {
        if (isSecretSiteUnlocked)
        {
            TextMeshProUGUI tmp = SecreteSiteBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = SecreteSiteDescription;
        }
    }

    void ChangeHammerSawDescriptionIfUnlocked()
    {
        if (isHammerSawUnlocked)
        {
            TextMeshProUGUI tmp = HammerSawBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = HammerSawDescription;
        }
    }

    void ChangeStrawCraftDescriptionIfUnlocked()
    {
        if (isStrawCraftUnlocked)
        {
            TextMeshProUGUI tmp = StrawCraftBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = StrawCraftDescription;
        }
    }

    void ChangeAltarDescriptionIfUnlocked()
    {
        if (isAltarUnlcoked)
        {
            TextMeshProUGUI tmp = AltarBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = AltarDescription;
        }
    }

    void ChangeDrumDescriptionIfUnlocked()
    {
        if (isDrumUnlocked)
        {
            TextMeshProUGUI tmp = DrumBuildingBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = DrumDescription;
        }
    }

}
