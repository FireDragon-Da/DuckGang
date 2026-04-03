using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeetingManager : MonoBehaviour
{
    public static MeetingManager reference;
    [SerializeField] ThoughtSelectManager thoughtSelectManager;

    [SerializeField] List<DuckThought> allThoughts;

    List<DuckThought> curThoughts = new();
    public List<DuckThought> CurThoughts => curThoughts;

    [SerializeField] int optionCount = 3;

    //TODO prob clean this up
    public bool hasSerfdomSystem;
    public bool hasCompassionateSociety;
    public bool hasGatherSociety;
    public bool hasBeneficialSocialInteraction;
    public bool hasRomanticSociety;
    public bool hasCrumbieAllocationSystem;
    public bool hasStrongAttitude;

    void Awake()
    {
        reference = this;
        gameObject.SetActive(false);
    }

    public void StartMeeting()
    {
        List<DuckThought> optionThoughts = new();

        //Grab options
        for (int i = 0; i < optionCount; i++)
        {
            int chosenNum = Random.Range(0, allThoughts.Count);
            optionThoughts.Add(allThoughts[chosenNum]);
            allThoughts.RemoveAt(chosenNum);
        }

        optionThoughts.AddRange(curThoughts);
        curThoughts.Clear();

        for (int i = 0; i < thoughtSelectManager.options.Length; i++)
        {
            if (i < optionThoughts.Count) {
                thoughtSelectManager.options[i].thought = optionThoughts[i];
            }
            else
            {
                thoughtSelectManager.options[i].thought = null;
            }
            thoughtSelectManager.options[i].SetupButton();
        }

        thoughtSelectManager.maxSelections = optionCount;
        gameObject.SetActive(true);

        TimeManager.reference.AddPause();
    }

    public void EndMeeting()
    {
        for (int i = 0; i < thoughtSelectManager.options.Length; i++)
        {
            if (thoughtSelectManager.options[i].ActuallyOn)
            {
                curThoughts.Add(thoughtSelectManager.options[i].thought);
            }
            else
            {   //Re-add options back to full list
                if (thoughtSelectManager.options[i].thought != null) {
                    allThoughts.Add(thoughtSelectManager.options[i].thought);
                }
            }
        }

        thoughtSelectManager.ResetSelections();

        ProcessThoughts();
        gameObject.SetActive(false);

        TimeManager.reference.RemovePause();
    }

    void ProcessThoughts()
    {
        //TODO clean this up

        hasSerfdomSystem = false;
        hasCompassionateSociety = false;
        hasGatherSociety = false;
        hasBeneficialSocialInteraction = false;
        hasRomanticSociety = false;
        hasCrumbieAllocationSystem = false;
        hasStrongAttitude = false;


        for (int i = 0; i < curThoughts.Count; i++)
        {
            if (curThoughts[i] == null) {continue;}

            switch (curThoughts[i].Type)
            {
                case DuckThought.ThoughtType.SerfdomSystem:
                    hasSerfdomSystem = true;
                    break;
                case DuckThought.ThoughtType.CompassionateSociety:
                    hasCompassionateSociety = true;
                    break;
                case DuckThought.ThoughtType.GatherSociety:
                    hasGatherSociety = true;
                    break;
                case DuckThought.ThoughtType.BeneficialSocialInteraction:
                    hasBeneficialSocialInteraction = true;
                    break;
                case DuckThought.ThoughtType.RomanticSociety:
                    hasRomanticSociety = true;
                    break;
                case DuckThought.ThoughtType.CrumbieAllocationSystem:
                    hasCrumbieAllocationSystem = true;
                    break;
                case DuckThought.ThoughtType.StrongAttitude:
                    hasStrongAttitude = true;
                    break;
            }
        } 
    }

}
