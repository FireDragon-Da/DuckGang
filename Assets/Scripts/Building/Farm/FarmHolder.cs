using UnityEngine;

public class FarmHolder : Building
{
    [SerializeField] Farmland[] farmland;

    public bool destroyed; //To avoid infinite loop with child destruction

    //When built, build child farms
    public override void Build()
    {
        base.Build();
        foreach (Farmland farmPiece in farmland)
        {
            farmPiece.Build();   
        }
    }

    //When removed, remove child farms
    public override void Remove()
    {
        destroyed = true;
        foreach (Farmland farmPiece in farmland)
        {
            farmPiece.Remove();   
        }
        base.Remove();
    }

}
