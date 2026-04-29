using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPageGen : MonoBehaviour
{
    public static ConstructionPageGen reference;

    [SerializeField] GameObject content;
    [SerializeField] GameObject buildBar;
    List<GameObject> buildingListGO;

    private void Awake()
    {
        if (reference == null) reference = this;
        else Destroy(this);
    }

    void Start()
    {

        buildingListGO = UnlockingUIManager.reference.buildingListGO;

        foreach (GameObject g in buildingListGO)
        {
            Building b = g.GetComponent<Building>();
            
            GameObject newBar = Instantiate(buildBar, content.transform);

            newBar.GetComponent<BuildingBar>().myBuilding = b;
            newBar.GetComponentInChildren<TextMeshProUGUI>().text = UnlockingUIManager.reference.lockedString;
            newBar.GetComponent<Button>().interactable = false;

            Transform imageTransform = newBar.transform.Find("Image");
            imageTransform.gameObject.GetComponent<Image>().sprite= b.SpriteRenderer.sprite;

            UnlockingUIManager.reference.buildingBars.Add(b.buildingName, newBar);
            
        }

       this.gameObject.SetActive(false);
    }
}
