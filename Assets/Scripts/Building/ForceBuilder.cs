using UnityEngine;

public class ForceBuilder : MonoBehaviour
{
    void Start()
    {
        Building building = GetComponent<Building>();

        building.Build();

        Vector2Int arrayPos = MapManager.reference.TilemapPosToArrayPos(building.GetBottomLeftTile());

        for (int i = 0; i < building.Width; i++)
        {
            for (int j = 0; j < building.Height; j++)
            {
                MapManager.reference.buildingArray[arrayPos.x+i,arrayPos.y-j] = building;
            }
        }
    }
}
