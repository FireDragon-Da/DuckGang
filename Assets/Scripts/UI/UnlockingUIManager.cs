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

    private bool unlockAllForDebug = false;
    [HideInInspector] public string lockedString;

    private void Awake()
    {
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
                b.unlocked = true;
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

    //this MUST be tied to the open construction button!! if it isn't literally nothing will work!!!!
    public void checkAllUnlocks()
    {
        foreach (Building b in buildingList)
        {
            if (b.checkIfUnlocked()) { updateBuildMenu(b); }
        }
    }

    //this is used by building!
    public void triggerPopup(string txt)
    {
        List<PopupMessageData> popupTextList = new();
        popupTextList.Add(new PopupMessageData(txt));
        PopupManager.Instance.StartPopupSequence(popupTextList);
    }

    //unlock and change description - ONLY triggers when build menu is opened
    void updateBuildMenu(Building b)
    {
        b.buildingBar.GetComponentInChildren<TextMeshProUGUI>().text = FormatDescription(b);

        Button btn = b.buildingBar.GetComponent<Button>();
        btn.interactable = true;

        Transform imageTransform = b.buildingBar.transform.Find("Image");
        imageTransform.gameObject.SetActive(true);
    }
}
