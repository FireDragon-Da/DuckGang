using System.Collections;
using UnityEngine;

public class Obstacle : Building
{
    float actualRemoveHitsRequired => removeHitsRequired - UpgradeMeetingManager.reference.ObstacleDestructionReduction;

    protected override void Start()
    {
        BasicBuild();
        UpdateBuildingGrid();
        useInteractBounce = false;
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

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        continueBehavior = false;
        PlayInteractBounce();
        if (removing)
        {
            if (vfxHandler != null) vfxHandler.PlayEffect(removeHitVFX);

            //Add 1 remove
            yield return StartCoroutine(WaitWithProgress(removeTime, duck.ProgressBar));

            AddRemove();
            if (removeCounter >= actualRemoveHitsRequired)
            {
                Remove();
            }

            yield break;
        }
    }

    void AddRemove()
    {
        removeCounter++;

        progressBar.ChangeFill((float)removeCounter/actualRemoveHitsRequired);
    }
}
