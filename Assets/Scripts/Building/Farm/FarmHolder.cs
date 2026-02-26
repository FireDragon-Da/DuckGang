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
}
