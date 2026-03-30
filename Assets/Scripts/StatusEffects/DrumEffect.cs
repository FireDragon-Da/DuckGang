using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DrumEffect", menuName = "StatusEffect/DrumEffect")]
public class DrumEffect : StatusEffect
{
    [SerializeField] float speedPercentIncrease;
    DuckWalk target;

    public override void Added(DuckWalk duck)
    {
        base.Added(duck);
        duck.GainSpeedModifier(speedPercentIncrease);
        target = duck;
    }

    public override void Removed()
    {
        target.GainSpeedModifier(-speedPercentIncrease);
        base.Removed();
    }

    public override void DuplicateGained()
    {
        base.DuplicateGained();
        target.GainSpeedModifier(-speedPercentIncrease);
    }

}
