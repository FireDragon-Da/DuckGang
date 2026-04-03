using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DuckThought", menuName = "DuckThought")]
public class DuckThought : ScriptableObject
{

    public enum ThoughtType
    {
        SerfdomSystem,
        CompassionateSociety,
        GatherSociety,
        BeneficialSocialInteraction,
        RomanticSociety,
        CrumbieAllocationSystem,
        StrongAttitude,

    }

    [SerializeField] string thoughtText;
    public string ThoughtText => thoughtText;
    [SerializeField] ThoughtType type;
    public ThoughtType Type => type;

}
