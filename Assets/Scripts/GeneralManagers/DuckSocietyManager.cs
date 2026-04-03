using System.Collections.Generic;
using UnityEngine;

public class DuckSocietyManager : MonoBehaviour
{
    public static DuckSocietyManager reference;

    [SerializeField] GameObject duckPrefab;

    [Header("NewspaperData")]
    public List<string> newbornDuckNames = new();
    public List<DeathEvent> recentDeaths = new();
    public List<ArticleEvent> articles = new();

    void Awake()
    {
        reference = this;
    }

    public GameObject SpawnDuck(Vector2 position)
    {

        int totalHappiness = 0;
        foreach (GameObject duck in PublicInfo.reference.duckList)
        {
            totalHappiness += duck.GetComponent<DuckStats>().Happiness;
        }

        int averageHappiness = totalHappiness / PublicInfo.reference.duckList.Count;

        GameObject newDuck = Instantiate(duckPrefab, new(position.x, position.y), new Quaternion());
        newbornDuckNames.Add(newDuck.GetComponent<DuckNameGen>().CurrentDuckName);

        newDuck.GetComponent<DuckStats>().SetHappiness(averageHappiness);

        return newDuck;
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