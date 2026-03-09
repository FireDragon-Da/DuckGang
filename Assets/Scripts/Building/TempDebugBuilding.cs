using UnityEngine;

public class TempDebugBuilding : Building
{
    //TODO delete this script and remove references, this shouldn't be used in its current state
    void Start()
    {
        Vector2Int arrayPos = MapManager.reference.TilemapPosToArrayPos(new((int)(transform.position.x-0.5f),(int)(transform.position.y-1.5f)));
        print(arrayPos);
        MapManager.reference.buildingArray[arrayPos.x,arrayPos.y] = this;
    }

}
