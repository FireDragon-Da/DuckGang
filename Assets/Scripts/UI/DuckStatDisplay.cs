using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DuckStatDisplay : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nameAndAge;
    [SerializeField] TextMeshProUGUI hunger;
    [SerializeField] TextMeshProUGUI happiness;

    string duckName;
    DuckStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            updateStats();
        }
    }

    public void displayStats(string name, DuckStats duckStats)
    {
        gameObject.SetActive(true);
        duckName = name;
        stats = duckStats;
    }

    void updateStats()
    {
        nameAndAge.text = duckName + ", " + stats.Age;
        hunger.text = "hunger: " + stats.Hunger + "/100";
        happiness.text = "happiness: " + stats.Happiness + "/100";
    }


    //obselete
    public void displayStats(string name, int age, int hun, int hap)
    {
        gameObject.SetActive(true);
        nameAndAge.text = name + ", " + age;
        hunger.text = "hunger: " + hun + "/100";
        happiness.text = "happiness: " + hap + "/100";
    }

}
