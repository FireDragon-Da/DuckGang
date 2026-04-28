using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeMeetingManager : MonoBehaviour
{
    public static UpgradeMeetingManager reference;

    [SerializeField] List<DuckUpgrade> allUpgrades;
    public List<DuckUpgrade> AllUpgrades => allUpgrades;
    [SerializeField] int optionCount = 3;
    List<DuckUpgrade> optionUpgrades = new();

    int curSelected;

    [SerializeField] List<UpgradeButton> upgradeButtons;

    public float SpeedIncrease = 1;
    public float LoveIncrease;
    public float SaddnessResistance = 1;
    public float HungerResistance = 1;
    public int BuildingDecrease;
    public int ObstacleDestructionReduction;
    public int FarmlandBuff;
    public int HappinessIncrease;
    public int NestDurability;
    public float FoodBuff = 1;
    public int StrawCraftBuff;
    public int AltarBuff;
    public int DucksonSiteBuff;

    void Awake()
    {
        reference = this;
        gameObject.SetActive(false);
    }

    public void StartMeeting()
    {
        optionUpgrades.Clear();
        List<DuckUpgrade> possibleChoices = new();
        possibleChoices.AddRange(allUpgrades);

        //Grab options
        for (int i = 0; i < optionCount; i++)
        {
            int chosenNum = Random.Range(0, possibleChoices.Count);
            optionUpgrades.Add(possibleChoices[chosenNum]);
            possibleChoices.RemoveAt(chosenNum);
        }

        //Ready buttons
        for (int i = 0; i < optionCount; i++)
        {
            upgradeButtons[i].upgrade = optionUpgrades[i];
            upgradeButtons[i].SetupButton();
        }

        curSelected = -1;

        gameObject.SetActive(true);

        TimeManager.reference.AddPause();
    }

    void EndMeeting(int choice)
    {
        LevelUp(optionUpgrades[choice]);
        gameObject.SetActive(false);

        TimeManager.reference.RemovePause();
    }

    public void SelectUpgrade(int index)
    {
        curSelected = index;
    }

    public void EndMeetingButton()
    {
        if (curSelected != -1)
        {
            EndMeeting(curSelected);
        }
    }

    public void LevelUp(DuckUpgrade target)
    {
        target.LevelUp();
        switch (target.Type)
        {
            case DuckUpgrade.UpgradeType.SpeedIncrease:
                SpeedIncrease += 0.05f;
                break;
            case DuckUpgrade.UpgradeType.LoveIncrease:
                LoveIncrease += 0.05f;
                break;
            case DuckUpgrade.UpgradeType.SaddnessResistance:
                SaddnessResistance /= 1.05f;
                break;
            case DuckUpgrade.UpgradeType.HungerResistance:
                HungerResistance /= 1.05f;
                break;
            case DuckUpgrade.UpgradeType.BuildingDecrease:
                BuildingDecrease++;
                break;
            case DuckUpgrade.UpgradeType.ObstacleDestructionReduction:
                ObstacleDestructionReduction -= 2;
                break;
            case DuckUpgrade.UpgradeType.FarmlandBuff:
                FarmlandBuff++;
                break;
            case DuckUpgrade.UpgradeType.HappinessIncrease:
                HappinessIncrease += 5;
                break;
            case DuckUpgrade.UpgradeType.NestDurability:
                NestDurability++;
                break;
            case DuckUpgrade.UpgradeType.FoodBuff:
                FoodBuff *= 1.05f;
                break;
            case DuckUpgrade.UpgradeType.StrawCraftBuff:
                StrawCraftBuff += 2;
                break;
            case DuckUpgrade.UpgradeType.AltarBuff:
                AltarBuff += 10;
                break;
            case DuckUpgrade.UpgradeType.DucksonSiteBuff:
                DucksonSiteBuff += 25;
                break;
        }
    }

}
