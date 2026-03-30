using System.Collections.Generic;
using UnityEngine;

public class MeetingManager : MonoBehaviour
{
    public static MeetingManager reference;

    [SerializeField] List<DuckThought> allThoughts;

    List<DuckThought> curThoughts = new();
    public List<DuckThought> CurThoughts => curThoughts;

    void Awake()
    {
        reference = this;
    }

    public void StartMeeting()
    {
        
    }

    public void EndMeeting()
    {
        
    }
}
