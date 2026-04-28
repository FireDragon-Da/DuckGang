using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SecretSite : Building
{
    [Header("SecretSite")]
    [SerializeField] int takePerTouch = 10;
    int curCapactiy;
    [SerializeField] float fillTime = 1f;
    [SerializeField] int totalCapactiy = 100;
    [SerializeField] int totalGain = 150;
    bool producing;
    [SerializeField] float productionTime = 60f;
    [SerializeField] Sprite openSprite;
    [SerializeField] Sprite regularSprite;

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        duck.gameObject.GetComponentInChildren<DuckActionIndicator>().SetAction(DuckActionType.Invest);

        if (producing) {yield break;}

        if (CrumbManager.reference.ConsumeCrumbs(takePerTouch))
        {
            yield return StartCoroutine(WaitWithProgress(fillTime, duck.ProgressBar));
            curCapactiy += takePerTouch;
            CrumbManager.reference.SpawnCrumbiePopupDecrease(transform.position, takePerTouch);

            if (curCapactiy >= totalCapactiy)
            {
                producing = true;
                curCapactiy = 0;
                StartCoroutine(ProduceCrumbies());
            }
        }

        duck.gameObject.GetComponentInChildren<DuckActionIndicator>().SetAction(DuckActionType.None);


    }

    public IEnumerator ProduceCrumbies()
    {
        spriteRenderer.sprite = openSprite;

        progressBar.ShowBar();
        progressBar.ChangeFill(0);
        float elapsed = 0f;

        while (elapsed < productionTime)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / productionTime;
            progressBar.ChangeFill(progress);

            yield return null;
        }

        progressBar.HideBar();

        producing = false;
        CrumbManager.reference.GainCrumbs(totalGain);
        PlayInteractBounce();
        CrumbManager.reference.SpawnCrumbiePopupIncrease(transform.position, totalGain);

        spriteRenderer.sprite = regularSprite;
    }
}
