using UnityEngine;

public class Grass : Building , Farmlike
{

    int hits = 0;
    int maxHits;

    int compostBoost;

    //Ideally this stuff would have been done in mapgen
    protected override void Start()
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

            gain += compostBoost;

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

    public void GainBoost()
    {
        compostBoost++;
    }

    public void RemoveBoost()
    {
        compostBoost--;
    }

}
