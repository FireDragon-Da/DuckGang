using UnityEngine;

public class FarmHolder : Building
{
    [SerializeField] Farmland[] farmland;

    //When built, build child farms
    public override void Build()
    {
        base.Build();
        foreach (Farmland farmPiece in farmland)
        {
            farmPiece.Build();   
        }
    }

    public override void StartDeconstruction()
    {
        if (removing) {return;}

        base.StartDeconstruction();

        foreach (Farmland farmPiece in farmland)
        {
            farmPiece.Remove();   
        }
    }

    public override void StartBuild()
    {
        base.StartBuild();
        foreach (Farmland farmPiece in farmland)
        {
            farmPiece.StartBuild();   
        }
    }

    public override void TryStartRemove()
    {print("proc");
        if (interacting.Count == 0)
        {
            foreach (Farmland farm in farmland)
            {
                if (farm.Interacting.Count > 0)
                {
                    return;
                }
            }
            StartDeconstruction();
        }
    }

}
