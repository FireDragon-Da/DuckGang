using System.Collections.Generic;
using UnityEngine;

public class UpdateLog : MonoBehaviour
{
    [SerializeField] List<UpdateLogEntry> logEntries;

    public static UpdateLog reference;

    void Awake()
    {
        reference = this;
        
    }

    void Start()
    {
        RefreshList();
        gameObject.SetActive(false);
    }

    public void RefreshList()
    {
        int next = 0;
        foreach (DuckUpgrade upgrade in UpgradeMeetingManager.reference.AllUpgrades)
        {
            if (upgrade.Level > 0)
            {
                logEntries[next].Reload(upgrade);
                next++;
            }
        }

        for (int i = next; i < logEntries.Count; i++)
        {
            logEntries[i].MakeBlank();
        }
    }
}
