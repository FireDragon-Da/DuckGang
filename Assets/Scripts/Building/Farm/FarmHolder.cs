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
        base.StartDeconstruction();

        if (!removing) {return;}

        foreach (Farmland farmPiece in farmland)
        {
            farmPiece.StartDeconstruction();   
        }
    }

    public override void UnStartDeconstruction()
    {
        base.UnStartDeconstruction();

        foreach (Farmland farmPiece in farmland)
        {
            farmPiece.TryUnStartDeconstruction();   
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
    {
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

    public override void Remove()
    {
        PublicInfo.reference.constructionList.Remove(this);
        foreach (Farmland farmPiece in farmland)
        {
            farmPiece.Remove();   
        }
        Destroy(gameObject);
    }


}
