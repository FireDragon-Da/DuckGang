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

    public override IEnumerator BuildingInteract(DuckWalk duck)
    {
        yield return StartCoroutine(base.BuildingInteract(duck));
        if (!continueBehavior)
        {
            yield break;
        }

        if (producing) {yield break;}

        if (CrumbManager.reference.ConsumeCrumbs(takePerTouch))
        {
            yield return StartCoroutine(WaitWithProgress(fillTime, duck.ProgressBar));
            curCapactiy += takePerTouch;
            print("filed");
            if (curCapactiy >= totalCapactiy)
            {
                producing = true;
                curCapactiy = 0;
                StartCoroutine(ProduceCrumbies());
            }
        }
        
    }

    public IEnumerator ProduceCrumbies()
    {
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
    }
}
