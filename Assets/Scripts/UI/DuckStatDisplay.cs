using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DuckStatDisplay : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI age;
    [SerializeField] TextMeshProUGUI hunger;
    [SerializeField] TextMeshProUGUI happiness;

    string duckName;
    DuckStats stats;

    public static DuckStatDisplay reference;

    public bool displayExactNumbers = false;

    void Awake()
    {
        reference = this;
    }

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
        name.text = duckName;
        age.text = stats.Age/2 + " years old";

        if (displayExactNumbers)
        {
            hunger.text = "hunger: " + stats.Hunger + "/100";
            happiness.text = "joy: " + stats.Happiness + "/100";
        }
        else
        {
            string hungerText = "";
            string happinessText = "";

            switch (stats.Hunger)
            {
                case > 90:
                    hungerText = "Very full!";
                    break;
                case > 70:
                    hungerText = "Full & satisfied.";
                    break;
                case > 50:
                    hungerText = "Not too hungry.";
                    break;
                case > 30:
                    hungerText = "Feeling hungry.";
                    break;
                case > 10:
                    hungerText = "Very hungry...";
                    break;
                default:
                    hungerText = "STARVING";
                    break;
            }
            
            switch (stats.Happiness)
            {
                case > 90:
                    happinessText = "Euphoric!";
                    break;
                case > 70:
                    happinessText = "Very content.";
                    break;
                case > 50:
                    happinessText = "Quite happy.";
                    break;
                case > 30:
                    happinessText = "Feeling neutral.";
                    break;
                case > 10:
                    happinessText = "Getting sad...";
                    break;
                default:
                    happinessText = "Depressed";
                    break;
            }

            hunger.text = hungerText;
            happiness.text = happinessText;
        }
    }


}
