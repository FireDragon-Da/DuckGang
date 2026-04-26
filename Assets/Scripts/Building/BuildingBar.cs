using UnityEngine;
using UnityEngine.UI;

public class BuildingBar : MonoBehaviour
{

    public Building myBuilding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void onButtonClick()
    {
        BuildingPlacer.reference.UpdateBuildingPrefab(myBuilding);
        BuildingPlacer.reference.EnableBuild();
        GameObject.Find("GameManager").GetComponent<ObjectToggleAccessor>().ForceOn();
        TimeManager.reference.RemovePause();
        GameObject.Find("Construction_Page").GetComponent<ObjectToggleAccessor>().ForceOff();
    }
}
