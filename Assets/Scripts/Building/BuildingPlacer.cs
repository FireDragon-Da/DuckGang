using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] Building curBuildingPrefab;
    [SerializeField] GameObject buildingPreview;

    [SerializeField] Tilemap tilemap;

    int width;
    int height;

    void Start()
    {
        UpdateBuildingPrefab(curBuildingPrefab);
    }

    void Update()
    {
        if (active && !EventSystem.current.IsPointerOverGameObject())
        {
            buildingPreview.SetActive(true);

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseTile = tilemap.WorldToCell(worldPos);

            int startX = mouseTile.x - (width  - 1) / 2;
            int startY = mouseTile.y - (height - 1) / 2;

            Vector3Int bottomLeft = new Vector3Int(startX, startY, 0);
            Vector3 worldBottomLeft = tilemap.GetCellCenterWorld(bottomLeft);

            Vector3 offset = new Vector3((width - 1) / 2f, (height - 1) / 2f, 0f);
            Vector3 finalPosition = worldBottomLeft + offset;
            buildingPreview.transform.position = finalPosition;

            if (Input.GetMouseButtonDown(0) && IsPlacementValid(startX,startY)
                && CrumbManager.reference.ConsumeCrumbs(curBuildingPrefab.PlaceCost))
            {
                Building newBuilding = Instantiate(curBuildingPrefab, finalPosition, new());
                UpdateBuildingGrid(newBuilding, startX, startY);
            }
        }
        else
        {
            buildingPreview.SetActive(false);
        }
    }

    public void UpdateBuildingPrefab(Building newBuilding)
    {
        buildingPreview.GetComponent<SpriteRenderer>().sprite = newBuilding.SpriteRenderer.sprite;
        width = newBuilding.Width;
        height = newBuilding.Height;

        buildingPreview.transform.localScale = new(width,height,1);
    }

    void UpdateBuildingGrid(Building newBuilding, int startX, int startY)
    {
        for (int i = 0; i < newBuilding.Width; i++)
        {
            for (int j = 0; j < newBuilding.Height; j++)
            {
                Vector2Int arrayPos = MapManager.reference.TilemapPosToArrayPos(new(startX+i,startY+j));
                MapManager.reference.buildingArray[arrayPos.x,arrayPos.y] = newBuilding;
            }
        }
    }

    bool IsPlacementValid(int startX, int startY)
    {
        //TODO add land type checking

        for (int i = 0; i < curBuildingPrefab.Width; i++)
        {
            for (int j = 0; j < curBuildingPrefab.Height; j++)
            {
                Vector2Int arrayPos = MapManager.reference.TilemapPosToArrayPos(new(startX+i,startY+j));

                if (!MapManager.reference.IsArrayPosValid(arrayPos) || !MapManager.reference.IsBuildingPosEmpty(arrayPos))
                {
                    return false;
                }
            }
        }

        return true;
    }

    //Just for sprint demo2
    public void TempDemoFarmButton()
    {
        active = !active;
    }

}
