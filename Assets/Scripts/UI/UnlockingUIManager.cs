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
            .Replace("[b]", "\n\n\t" + totalCost)   
            .Replace("[c]", "\t\t" + (int)b.ConstructionNeeded);
    }

    public void checkAllUnlocks()
    {
        //calculate average happiness
        int happiness = 0;
        foreach (GameObject duck in PublicInfo.reference.duckList)
        {
            happiness += duck.GetComponent<DuckStats>().Happiness;
        }

        happiness /= PublicInfo.reference.duckList.Count;

        foreach (Building b in buildingList)
        {
            if (!b.unlocked)
            {
                if ((b.gameObject.GetComponent<FarmHolder>() && info.crumbieGainedFromGrass >= 10) ||
                    (b.gameObject.GetComponent<Building>().buildingName == "Nest" && buildingBars.Count > 0) ||
                    (b.gameObject.GetComponent<Playground>() && happiness <= 70) ||
                    (b.gameObject.GetComponent<GoldenCorn>() && info.farmList.Count >= (12 * 4)) ||
                    (b.gameObject.GetComponent<CompostSite>() && info.farmList.Count >= (5 * 4)) ||
                    (b.gameObject.GetComponent<SecretSite>() && CrumbManager.reference.Crumbs >= 200) ||
                    (b.gameObject.GetComponent<HammerSaw>() && info.curBuildingList.Count >= 20) ||
                    (b.gameObject.GetComponent<StrawCraft>() && info.crumbieGainedFromFarmland >= 500) ||
                    (b.gameObject.GetComponent<Altar>() && info.duckList.Count >= 50) ||
                    (b.gameObject.GetComponent<Drum>() && info.duckList.Count >= 5) ||
                    (b.gameObject.GetComponent<DiningHall>() && buildingBars.Count > 0) ||
                    (b.gameObject.GetComponent<Building>().buildingName == "InfiNest" && info.duckList.Count >= 25) ||
                    (b.gameObject.GetComponent<Building>().buildingName == "Statue" && CrumbManager.reference.Crumbs >= 1000))
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
