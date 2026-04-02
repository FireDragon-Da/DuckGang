using UnityEngine;

public class Obstacle : Building
{
    void Start()
    {
        BasicBuild();
        UpdateBuildingGrid();
    }

    void UpdateBuildingGrid()
    {
        Vector2Int tileBottomLeft = new Vector2Int(
            Mathf.FloorToInt(transform.position.x - width / 2f),
            Mathf.FloorToInt(transform.position.y - height / 2f)
        );

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (GetSpot(i,j)) {
                    Vector2Int arrayPos = MapManager.reference.TilemapPosToArrayPos(new(tileBottomLeft.x+i,tileBottomLeft.y+j));
                    MapManager.reference.buildingArray[arrayPos.x,arrayPos.y] = this;
                }
            }
        }
    }
}
