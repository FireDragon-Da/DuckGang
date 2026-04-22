using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPageGen : MonoBehaviour
{

    [SerializeField] GameObject content;
    [SerializeField] GameObject buildBar;
    List<Building> buildingList;
    List<GameObject> buildingListGO;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
    void Start()
    {/*
        buildingList = UnlockingUIManager.reference.buildingList;

        foreach (Building b in buildingList)
        {
            GameObject newBar = Instantiate(buildBar, content.transform);

            newBar.GetComponentInChildren<TextMeshProUGUI>().text = "IF THIS IS WORKING YOU'LL KNOW";
            newBar.GetComponentInChildren<Image>().sprite = b.gameObject.GetComponent<SpriteRenderer>().sprite;

            b.buildingBar = newBar;
        }
        */
        //gameobject ver

        buildingListGO = UnlockingUIManager.reference.buildingListGO;

        foreach (GameObject g in buildingListGO)
        {
            Building b = g.GetComponent<Building>();
            
            GameObject newBar = Instantiate(buildBar, content.transform);

            newBar.GetComponentInChildren<TextMeshProUGUI>().text = "IF THIS IS WORKING YOU'LL KNOW";
            newBar.GetComponentInChildren<Image>().sprite = b.SpriteRenderer.sprite;

            b.buildingBar = newBar;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
