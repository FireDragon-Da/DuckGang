using System.Collections.Generic;
using UnityEngine;

public class CompostSite : Building
{

    [SerializeField] float defaultDecayTimer;
    float decayTimer;
    [SerializeField] float maxPoopCount;
    int poopCount;

    protected override void Update()
    {
        base.Update();

        if (poopCount <= 0) {return;}

        decayTimer -= Time.deltaTime;
        if (decayTimer <= 0)
        {
            poopCount--;
            if (poopCount > 0)
            {
                decayTimer += defaultDecayTimer;
            }
        }
    }

    public override void BuildingInteract(DuckWalk duck)
    {
        base.BuildingInteract(duck);

        if (poopCount == 0)
        {
            poopCount++;
            decayTimer = defaultDecayTimer;
        }
        else if (poopCount < maxPoopCount)
        {
            poopCount++;
        }
    }

}
