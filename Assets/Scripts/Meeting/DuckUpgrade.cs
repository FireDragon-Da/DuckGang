using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DuckUpgrade", menuName = "DuckUpgrade")]
public class DuckUpgrade : ScriptableObject
{

    public enum UpgradeType
    {
        SpeedIncrease,
        LoveIncrease,
        SaddnessResistance,
        HungerResistance,
        BuildingDecrease,
        ObstacleDestructionReduction,
        FarmlandBuff,
        HappinessIncrease,
        NestDurability,
        FoodBuff,
        StrawCraftBuff,
        AltarBuff,
        DucksonSiteBuff,
    }

    [SerializeField] string upgradeText;
    [SerializeField] string descriptionText;
    public string UpgradeText => upgradeText;
    public string DescriptionText => descriptionText;
    [SerializeField] UpgradeType type;
    public UpgradeType Type => type;

    int level;
    public int Level => level;

    public void LevelUp()
    {
        level++;
    }

}
