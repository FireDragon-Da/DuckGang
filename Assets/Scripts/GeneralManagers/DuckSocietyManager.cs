using System.Collections.Generic;
using UnityEngine;

public class DuckSocietyManager : MonoBehaviour
{
    public static DuckSocietyManager reference;

    [SerializeField] GameObject duckPrefab;

    [Header("NewspaperData")]
    public List<string> newbornDuckNames = new();
    public List<DeathEvent> recentDeaths = new();

    void Awake()
    {
        reference = this;
    }

    public void SpawnDuck(Vector2 position)
    {
        GameObject newDuck = Instantiate(duckPrefab, new(position.x, position.y), new Quaternion());
        newbornDuckNames.Add(newDuck.GetComponent<DuckNameGen>().CurrentDuckName);
    }

    public void ProcessDuckDeath(GameObject duck, DeathReason reason)
    {
        DuckNameGen nameGen = duck.GetComponent<DuckNameGen>();
        string duckName = nameGen != null ? nameGen.CurrentDuckName : "Unknown Duck";
        DeathEvent newEvent = new DeathEvent
        {
            duckName = duckName,
            reason = reason
        };
        recentDeaths.Add(newEvent);
        if (PublicInfo.reference != null && PublicInfo.reference.duckList != null)
        {
            PublicInfo.reference.duckList.Remove(duck);
        }

        Destroy(duck);
    }
}