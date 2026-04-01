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

}
