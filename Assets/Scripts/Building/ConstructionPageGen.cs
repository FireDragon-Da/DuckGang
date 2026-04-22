using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPageGen : MonoBehaviour
{

    [SerializeField] GameObject content;
    [SerializeField] GameObject buildBar;
    List<GameObject> buildingListGO; 
    
    void Start()
    {

        buildingListGO = UnlockingUIManager.reference.buildingListGO;

        foreach (GameObject g in buildingListGO)
        {
            Building b = g.GetComponent<Building>();
            
            GameObject newBar = Instantiate(buildBar, content.transform);
            b.buildingBar = newBar;

            newBar.GetComponentInChildren<TextMeshProUGUI>().text = UnlockingUIManager.reference.lockedString;

            Transform imageTransform = b.buildingBar.transform.Find("Image");
            imageTransform.gameObject.GetComponent<Image>().sprite= b.SpriteRenderer.sprite;
            
        }

        this.gameObject.SetActive(false);
    }
}
