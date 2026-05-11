using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

//looking for a bunch of code that isn't here anymore? 
//most of it is now being handled either within the Building script, or a new script called ConstructionPageGen,
//and the specific unlock conditions are handled in individual building scripts

public class UnlockingUIManager : MonoBehaviour
{

    public static UnlockingUIManager reference;
    public QuacxiconSO quacxiconSO;
    PublicInfo info;

    public List<GameObject> buildingListGO = new();
    public List<Building> buildingList = new();

    public Dictionary<string, GameObject> buildingBars = new();

    public bool unlockAllForDebug = false;
    [HideInInspector] public string lockedString;

    private void Awake()
    {
        if (reference == null) reference = this;
        else Destroy(this);

        lockedString = quacxiconSO.GetRandomLogFromCategory("Locked");
    }

    void Start()
    {
        info = PublicInfo.reference;

        foreach(GameObject g in buildingListGO)
        {
            Building b = g.GetComponent<Building>();

            buildingList.Add(b);

            b.unlocked = false;

            PublicInfo.reference.buildingEverBuilt.Add(b.buildingName, false);
        }

        if (unlockAllForDebug)
        {
            foreach (Building b in buildingList)
            {
                updateBuildMenu(b);
            }
        }
    }

    string FormatDescription(Building b, string s)
    {
        int totalCost = (int)(b.ConstructionNeeded * b.BuildCost) + b.PlaceCost;

        return s
            .Replace("[b]", (int)b.ConstructionNeeded + " effort needed")        
            .Replace("[c]", totalCost + " crumbs needed");
    }

    public void checkAllUnlocks()
    {
        foreach (Building b in buildingList)
        {
            if (!b.unlocked)
            {
                if ((b.gameObject.GetComponent<FarmHolder>() && info.crumbieGainedFromGrass >= 10) ||
                    (b.gameObject.GetComponent<Nest>() && info.farmList.Count >= 1) ||
                    (b.gameObject.GetComponent<Playground>() && info.crumbieEverCollected >= 50) ||
                    (b.gameObject.GetComponent<GoldenCorn>() && info.farmList.Count >= 8) ||
                    (b.gameObject.GetComponent<CompostSite>() && info.farmList.Count >= 12) ||
                    (b.gameObject.GetComponent<SecretSite>() && info.crumbieEverCollected >= 300) ||
                    (b.gameObject.GetComponent<HammerSaw>() && info.curBuildingList.Count >= 5) ||
                    (b.gameObject.GetComponent<StrawCraft>() && info.crumbieGainedFromFarmland >= 30) ||
                    (b.gameObject.GetComponent<Altar>() && info.duckList.Count >= 35) ||
                    (b.gameObject.GetComponent<Drum>() && info.duckCollideBuildingTimes >= 70) ||
                    (b.gameObject.GetComponent<DiningHall>() && buildingBars.Count > 0) ||
                    (b.gameObject.GetComponent<Building>().buildingName == "InfiNest" && info.duckList.Count >= 20) ||
                    (b.gameObject.GetComponent<Building>().buildingName == "Statue" && CrumbManager.reference.Crumbs > 9999))
                {
                    updateBuildMenu(b);
                }
            }
        }
    }

    //this is used by building!
    public void triggerPopup(string txt)
    {
        if (txt == "") {return;}
        
        List<PopupMessageData> popupTextList = new();
        popupTextList.Add(new PopupMessageData(txt));
        PopupManager.Instance.StartPopupSequence(popupTextList);
    }

    //unlock and change description - ONLY triggers when build menu is opened
    public void updateBuildMenu(Building b)
    {
        b.unlocked = true;

        GameObject bar = buildingBars[b.buildingName];

        bar.GetComponentInChildren<TextMeshProUGUI>().text = FormatDescription(b, quacxiconSO.GetSpecificLogFromCategory(b.buildingName, 1));

        bar.GetComponent<Button>().interactable = true;

        Transform imageTransform = bar.transform.Find("Image");
        imageTransform.gameObject.SetActive(true);

        triggerPopup(quacxiconSO.GetSpecificLogFromCategory(b.buildingName, 2));
    }

    private void Update()
    {
        checkAllUnlocks();
    }
}
