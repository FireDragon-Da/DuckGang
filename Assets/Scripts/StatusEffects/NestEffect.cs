using UnityEngine;

public class NestEffect : StatusEffect
{

    public override Vector2 Activate(GameObject target)
    {
        int num = Random.Range(0,PublicInfo.reference.duckList.Count);
        return PublicInfo.reference.duckList[num].transform.position - target.transform.position;
    }


    public override bool EffectTried()
    {
        return true;
    }

}
