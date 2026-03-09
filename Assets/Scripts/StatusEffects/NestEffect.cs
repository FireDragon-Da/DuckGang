using UnityEngine;

[CreateAssetMenu(fileName = "NestEffect", menuName = "StatusEffect/NestEffect")]
public class NestEffect : StatusEffect
{

    public override Vector2 Activate(GameObject target)
    {
        int num = Random.Range(0,PublicInfo.reference.duckList.Count);

        if (PublicInfo.reference.duckList[num] == target)
        {
            num += 1;
            if (num == PublicInfo.reference.duckList.Count)
            {
                num = 0;
            }
        }

        Vector2 output = PublicInfo.reference.duckList[num].transform.position - target.transform.position;


        if (output == Vector2.zero)
        {
            return Vector2.up;
        }
        else
        {
            return output.normalized;
        }
    }


    public override bool EffectTried()
    {
        return true;
    }

}
