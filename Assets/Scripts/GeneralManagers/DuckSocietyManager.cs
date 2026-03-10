using System.Collections.Generic;
using UnityEngine;

public class DuckSocietyManager : MonoBehaviour
{
    public static DuckSocietyManager reference;
    
    [SerializeField] GameObject duckPrefab;

    [Header("NewspaperData")]
    public List<string> newbornDuckNames;
    public List<DeathEvent> recentDeaths;

    void Awake()
    {
        reference = this;
    }

    public void SpawnDuck(Vector2 position)
    {
        GameObject newDuck = Instantiate(duckPrefab, new(position.x, position.y), new Quaternion());

        newbornDuckNames.Add(newDuck.GetComponent<DuckNameGen>().CurrentDuckName);
    }

}
