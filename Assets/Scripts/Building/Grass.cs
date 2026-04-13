using System;
using UnityEngine;

public class Grass : Building
{
    [Header("Grass")]
    [SerializeField] float growTime;
    [SerializeField] Sprite grownSprite;
    [SerializeField] Sprite emptySprite;
    float curGrowTimer;

    int hits = 0;
    int maxHits;
    bool hasFood;

    //Ideally alot of this stuff would have been done in mapgen
    protected override void Start()
    {
        Vector2Int arrayPos = MapManager.reference.TilemapPosToArrayPos(
            MapManager.reference.TransformPosToTilemapPos(transform.position)
        );
        MapManager.reference.buildingArray[arrayPos.x,arrayPos.y] = this;

        PublicInfo.reference.grassList.Add(this);

        BasicBuild();

        maxHits = TuningManager.reference.maxGrassCrumbs;
        hasFood = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            
            if (collision.GetComponent<DuckWalk>().beingDragged)
            {
                return;
            }

            if (!hasFood)
            {
                return;
            }

            int gain = 1;
            
            if (MeetingManager.reference.hasGatherSociety)
            {
                gain += 1;
            }

            CrumbManager.reference.GainCrumbs(gain);
            PublicInfo.reference.crumbieGainedFromGrass += gain;
            SoundSystem.instance.PlaySound("grass");
            CrumbManager.reference.SpawnCrumbiePopupIncrease(transform.position, gain);

            hits++;
            if (hits >= maxHits)
            {
                hasFood = false;
                curGrowTimer = growTime;
                spriteRenderer.sprite = emptySprite;
            }
        }
    }

    protected override void UpdateBehavior()
    {
        base.UpdateBehavior();

        if (curGrowTimer > 0)
        {
            curGrowTimer -= Time.deltaTime;
            if (curGrowTimer <= 0)
            {
                hits = 0;
                hasFood = true;
                spriteRenderer.sprite = grownSprite;
            }
        }
    }
}
