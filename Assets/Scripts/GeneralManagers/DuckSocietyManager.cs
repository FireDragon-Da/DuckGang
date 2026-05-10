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

    [Header("Death Announcement")]
    [SerializeField] private QuacxiconSO quacxiconSO;
    [SerializeField] private string deathCategoryName = "Death";

    void Awake()
    {
        reference = this;
    }

    void OnDestroy()
    {
        if (reference == this)
        {
            reference = null;
        }
    }

    public GameObject SpawnDuck(Vector2 position)
    {

        int totalHappiness = 0;
        foreach (GameObject duck in PublicInfo.reference.duckList)
        {
            totalHappiness += duck.GetComponent<DuckStats>().Happiness > 40 ? duck.GetComponent<DuckStats>().Happiness : 50 ;
        }

        int averageHappiness = totalHappiness / PublicInfo.reference.duckList.Count;

        GameObject newDuck = Instantiate(duckPrefab, new(position.x, position.y), new Quaternion());

        DuckNameGen nameGen = newDuck.GetComponent<DuckNameGen>();
        nameGen.GenerateName();
        newbornDuckNames.Add(nameGen.CurrentDuckName);

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

        if (TextBox.reference != null)
        {
            string deathLine = null;

            if (quacxiconSO != null)
            {
                string categoryToSearch = "";
                switch (reason)
                {
                    case DeathReason.OldAge:
                        categoryToSearch = "Death_OldAge";
                        break;
                    case DeathReason.Starvation:
                        categoryToSearch = "Death_Starvation";
                        break;
                    case DeathReason.Disappeared:
                        categoryToSearch = "Death_Disappeared";
                        break;
                    case DeathReason.Suicide:
                        categoryToSearch = "Death_Suicide";
                        break;
                    default:
                        categoryToSearch = "Death";
                        break;
                }

                deathLine = quacxiconSO.GetRandomLogFromCategory(categoryToSearch);
            }

            string message;
            if (string.IsNullOrEmpty(deathLine))
                message = $"<color=#B22727>{duckName} has died.</color>";
            else
                message = $"<color=#B22727>{duckName} {deathLine}</color>";

            TextBox.reference.gameObject.SetActive(true);
            TextBox.reference.AddLine(message);
        }

        if (PublicInfo.reference != null && PublicInfo.reference.duckList != null)
        {
            PublicInfo.reference.duckList.Remove(duck);
        }

        Destroy(duck);
    }
}