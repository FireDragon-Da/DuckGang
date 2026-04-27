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
        optionUpgrades[choice].LevelUp();
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

}
