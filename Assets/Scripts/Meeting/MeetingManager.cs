using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeetingManager : MonoBehaviour
{
    public static MeetingManager reference;

    [SerializeField] List<DuckThought> allThoughts;

    List<DuckThought> curThoughts = new();
    public List<DuckThought> CurThoughts => curThoughts;

    List<DuckThought> optionThoughts = new();

    [SerializeField] int optionCount = 3;

    void Awake()
    {
        reference = this;
    }

    public void StartMeeting()
    {
        //Grab options
        for (int i = 0; i < optionCount; i++)
        {
            int chosenNum = Random.Range(0, allThoughts.Count);
            optionThoughts.Add(allThoughts[chosenNum]);
            allThoughts.RemoveAt(chosenNum);
        }
    }

    public void EndMeeting()
    {
        
        //Re-add options back to full list
        for (int i = 0; i < optionThoughts.Count; i++)
        {
            allThoughts.Add(optionThoughts[i]);
        }
        optionThoughts.Clear();
    }
}
