using UnityEngine;

public class Grass : Building
{

    int hits = 0;
    int maxHits;

    //Ideally this stuff would have been done in mapgen
    void Start()
    {
        Vector2Int arrayPos = MapManager.reference.TilemapPosToArrayPos(
            MapManager.reference.TransformPosToTilemapPos(transform.position)
        );
        MapManager.reference.buildingArray[arrayPos.x,arrayPos.y] = this;

        PublicInfo.reference.grassList.Add(this);

        BasicBuild();

        maxHits = TuningManager.reference.maxGrassCrumbs;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            
            if (collision.GetComponent<DuckWalk>().beingDragged)
            {
                return;
            }

            int gain = 1;

            if (hits > maxHits && !MeetingManager.reference.hasGatherSociety) {
                return;
            }
            
            if (hits <= maxHits && MeetingManager.reference.hasGatherSociety)
            {
                gain += 1;
            }

            CrumbManager.reference.GainCrumbs(gain);
            PublicInfo.reference.crumbieGainedFromGrass += gain;
            SoundSystem.instance.PlaySound("grass");
            CrumbManager.reference.SpawnCrumbiePopupIncrease(transform.position, gain);

            hits++;
        }
    }
}
