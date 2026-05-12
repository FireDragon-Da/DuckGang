using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;

using NUnit.Framework;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] List<Building> buildingList;
    [SerializeField] GameObject buildingPreview;
    SpriteRenderer previewSprite;

    [SerializeField] Tilemap tilemap;

    int width;
    int height;

    bool removeToggled;
    Building curBuildingPrefab;

    public static BuildingPlacer reference;
    public bool Using => active || removeToggled;

    void Awake()
    {
        reference = this;
    }

    void Start()
    {
        previewSprite = buildingPreview.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if ((active || removeToggled) && Input.GetMouseButtonDown(1))
        {
            DisableBuildAndRemove();
        }
        else if (removeToggled && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits;

            if (Input.GetMouseButtonDown(0))
            {
                hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.CompareTag("Building"))
                    {
                        hit.collider.GetComponent<Building>().TryStartRemove();
                    }
                }


            }
        }
        else if (active && Input.GetMouseButtonDown(1))
        {
            DisableBuildAndRemove();
        }
        else if (active && !EventSystem.current.IsPointerOverGameObject())
        {
            buildingPreview.SetActive(true);

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseTile = tilemap.WorldToCell(worldPos);

            int startX = mouseTile.x - (width - 1) / 2;
            int startY = mouseTile.y - (height - 1) / 2;

            Vector3Int bottomLeft = new Vector3Int(startX, startY, 0);
            Vector3 worldBottomLeft = tilemap.GetCellCenterWorld(bottomLeft);

            Vector3 offset = new Vector3((width - 1) / 2f, (height - 1) / 2f, 0f);
            Vector3 finalPosition = worldBottomLeft + offset;
            buildingPreview.transform.position = finalPosition;

            bool canPlace = IsPlacementValid(startX, startY) && 
                            CrumbManager.reference.Crumbs > curBuildingPrefab.PlaceCost;

            //Change visual color
            if (canPlace)
            {
                previewSprite.color = Color.white;
            }
            else
            {
                previewSprite.color = Color.red;
            }

            if (Input.GetMouseButtonDown(0) && canPlace
                && CrumbManager.reference.ConsumeCrumbs(curBuildingPrefab.PlaceCost))
            {
                Building newBuilding = Instantiate(curBuildingPrefab, finalPosition, new());

                //show the crumbie decrease popup animation
                CrumbManager.reference.SpawnCrumbiePopupDecrease(finalPosition, curBuildingPrefab.PlaceCost);
                //Place Sound
                SoundSystem.instance.PlaySound("place-building");

                UpdateBuildingGrid(newBuilding, startX, startY);
                newBuilding.StartBuild();
            }
        }
        else
        {
            buildingPreview.SetActive(false);
        }
    }

    public void UpdateBuildingPrefab(Building newBuilding)
    {
        curBuildingPrefab = newBuilding;
        buildingPreview.GetComponent<SpriteRenderer>().sprite = newBuilding.SpriteRenderer.sprite;
        width = newBuilding.Width;
        height = newBuilding.Height;
    }

    void UpdateBuildingGrid(Building newBuilding, int startX, int startY)
    {
        for (int i = 0; i < newBuilding.Width; i++)
        {
            for (int j = 0; j < newBuilding.Height; j++)
            {
                if (newBuilding.GetSpot(i,j)) {
                    Vector2Int arrayPos = MapManager.reference.TilemapPosToArrayPos(new(startX+i,startY+j));
                    MapManager.reference.buildingArray[arrayPos.x,arrayPos.y] = newBuilding;
                }
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

                if (!MapManager.reference.IsArrayPosValid(arrayPos) || !MapManager.reference.IsBuildingPosEmpty(arrayPos) || MapManager.reference.waterArray[arrayPos.x,arrayPos.y])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void EnableBuild()
    {
        active = true;
        TurnRemoveOff();
        MouseCursor.reference.SetSprite(MouseCursor.CursorType.None);
    }

    public void DisableBuildAndRemove()
    {
        active = false;
        TurnRemoveOff();
        MouseCursor.reference.SetSprite(MouseCursor.CursorType.Normal);
    }

    public void RemoveToggle()
    {
        if (removeToggled)
        {
            TurnRemoveOff();
        }
        else
        {
            TurnRemoveOn();
        }
        active = false;

        if (removeToggled)
        {
            MouseCursor.reference.SetSprite(MouseCursor.CursorType.Trash);
        }
        else
        {
            MouseCursor.reference.SetSprite(MouseCursor.CursorType.Normal);
        }
    }

    void TurnRemoveOn()
    {
        removeToggled = true;

        //Highlight all options red
        foreach (Building curBuilding in PublicInfo.reference.curBuildingList)
        {
            if (curBuilding == null) {continue;}

            if (curBuilding.CanRemoveInput)
            {
                curBuilding.SpriteRenderer.color = Color.red;
                curBuilding.FoundationSpriteRenderer.color = Color.red;
            }
        }

        foreach (Building curBuilding in PublicInfo.reference.constructionList)
        {
            if (curBuilding == null) {continue;}

            if (curBuilding.CanRemoveInput)
            {
                curBuilding.SpriteRenderer.color = Color.red;
                curBuilding.FoundationSpriteRenderer.color = Color.red;
            }
        }

        foreach (Obstacle curObstacle in PublicInfo.reference.obstacleList)
        {
            if (curObstacle == null) {continue;}
            
            if (curObstacle.CanRemoveInput)
            {
                curObstacle.SpriteRenderer.color = Color.red;
                curObstacle.FoundationSpriteRenderer.color = Color.red;
            }
        }

        foreach (Grass curGrass in PublicInfo.reference.grassList)
        {
            if (curGrass == null) {continue;}

            if (curGrass.CanRemoveInput)
            {
                curGrass.SpriteRenderer.color = Color.red;
                curGrass.FoundationSpriteRenderer.color = Color.red;
            }
        }
    }

    void TurnRemoveOff()
    {
        removeToggled = false;

        //Unhighlight all buildings
        foreach (Building curBuilding in PublicInfo.reference.curBuildingList)
        {
            if (curBuilding == null) {continue;}

            curBuilding.SpriteRenderer.color = Color.white;
            curBuilding.FoundationSpriteRenderer.color = Color.white;
        }

        foreach (Building curBuilding in PublicInfo.reference.constructionList)
        {
            if (curBuilding == null) {continue;}

            curBuilding.SpriteRenderer.color = Color.white;
            curBuilding.FoundationSpriteRenderer.color = Color.white;
        }

        foreach (Obstacle curObstacle in PublicInfo.reference.obstacleList)
        {
            if (curObstacle == null) {continue;}

            curObstacle.SpriteRenderer.color = Color.white;
            curObstacle.FoundationSpriteRenderer.color = Color.white;
        }

        foreach (Grass curGrass in PublicInfo.reference.grassList)
        {
            if (curGrass == null) {continue;}

            curGrass.SpriteRenderer.color = Color.white;
            curGrass.FoundationSpriteRenderer.color = Color.white;
        }
    }

}
