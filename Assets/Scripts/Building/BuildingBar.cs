using UnityEngine;
using UnityEngine.UI;

public class BuildingBar : MonoBehaviour
{

    public Building myBuilding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void onButtonClick()
    {
        print("placing!");
        BuildingPlacer.reference.UpdateBuildingPrefab(myBuilding);
        BuildingPlacer.reference.EnableBuild();

        if (GameObject.Find("Construction_Page") == null) { print("CAN'T FIND CONSTRUCTION_PAGE"); } else print("construction page found");
        if (GameObject.Find("GameManager").GetComponent<ObjectToggleAccessor>() == null) { print("GM doesn't have toggle accessor"); } else print("gm has ta");


        
        GameObject.Find("GameManager").GetComponent<ObjectToggleAccessor>().ForceOn();
        TimeManager.reference.RemovePause();

        GameObject.Find("Construction_Page").GetComponent<ObjectToggleAccessor>().ForceOff();
    }
}
