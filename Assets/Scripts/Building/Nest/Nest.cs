using UnityEngine;

public class Nest : Building
{
    bool empty;

    public bool Empty
    {
        get
        {
            return empty;
        }
    }

    public override void Build()
    {
        base.Build();

        PublicInfo.reference.nestList.Add(this);
    }

    public override void Remove()
    {
        PublicInfo.reference.nestList.Remove(this);
        base.Remove();
    }

}
