using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;

public enum ArticlePriority
{
    Fluff = 0,
    NewBuilding = 1,
    MentalHealthCrisis = 2,
    Starvation = 3,
    SocialFormation = 4
}

public enum DeathReason
{
    OldAge,
    Starvation,
    Disappeared,
    Suicide
}

public struct ArticleEvent
{
    public string title;
    public string content;
    public ArticlePriority priority;

    public ArticleEvent(string t, string c, string p)
    {
        title = t; content = c;
        switch (p)
        {

            case "MentalHealthCrisis":
                priority = ArticlePriority.MentalHealthCrisis;
                break;

            case "Starvation":
                priority = ArticlePriority.Starvation;
                break;

            case "NewBuilding":
                priority = ArticlePriority.NewBuilding;
                break;

            case "SocialFormation":
                priority = ArticlePriority.SocialFormation;
                break;

            default:
                priority = ArticlePriority.Fluff; 
                break;
        }
    }
}

public struct DeathEvent
{
    public string duckName;
    public DeathReason reason;
}

public class NewspaperController : MonoBehaviour
{
    public static NewspaperController reference;

    [Header("Data Source")]
    [SerializeField] private QuacxiconSO quacxiconSO;

    [Header("UI References - Header")]
    [SerializeField] private TMP_Text quoteText;
    [SerializeField] private TMP_Text changeInQuacklandText;

    [Header("UI References - Life & Death")]
    [SerializeField] private TMP_Text welcomeToLifeContent;
    [SerializeField] private TMP_Text goodbyeFromLifeContent;

    [Header("UI References - Articles")]
    [SerializeField] private TMP_Text topPriorityTitleText;
    [SerializeField] private TMP_Text topPriorityContentText;
    [SerializeField] private TMP_Text secondPriorityTitleText;
    [SerializeField] private TMP_Text secondPriorityContentText;

    void Awake()
    {
        if (reference == null)
        {
            reference = this;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateNewspaper(
        float currentHappiness,
        float currentHunger,
        string currentQuacklandChange,
        List<string> newbornDuckNames,
        List<DeathEvent> recentDeaths,
        List<ArticleEvent> currentEvents)
    {
        UpdateQuote(currentHappiness, currentHunger);
        UpdateChangeInQuackland(currentQuacklandChange);
        UpdateLifeSection(newbornDuckNames);
        UpdateDeathSection(recentDeaths);
        UpdateArticles(currentEvents);
    }

    public void UpdateQuote(float happiness, float hunger)
    {
        float averageStat = (happiness + hunger) / 2f;

        int quoteLevel = Mathf.Clamp(Mathf.FloorToInt(averageStat / 10f), 0, 9);

        print("happiness " + happiness + ", hunger " + hunger + ", quoteLevel " + quoteLevel);

        string categoryName = "Quotes";//$"Quote_Level_{quoteLevel}";
        string quoteStr = quacxiconSO.GetSpecificLogFromCategory(categoryName, quoteLevel);

        if (!string.IsNullOrEmpty(quoteStr))
        {
            quoteText.text = $"\"{quoteStr}\"";
        }
        else
        {
            quoteText.text = "\"Quack?\"";
        }
    }

    public void UpdateChangeInQuackland(string changeText)
    {
        changeInQuacklandText.text = changeText;
    }

    public void UpdateLifeSection(List<string> newbornDuckNames)
    {
        if (newbornDuckNames == null || newbornDuckNames.Count == 0)
        {
            welcomeToLifeContent.text = "No new ducks out recently.";
            return;
        }

        StringBuilder sb = new StringBuilder();
        foreach (string duckName in newbornDuckNames)
        {
            string birthAction = quacxiconSO.GetRandomLogFromCategory("BirthActions");
            sb.AppendLine($"{duckName} {birthAction}");
        }
        welcomeToLifeContent.text = sb.ToString();
    }

    public void UpdateDeathSection(List<DeathEvent> recentDeaths)
    {
        if (recentDeaths == null || recentDeaths.Count == 0)
        {
            goodbyeFromLifeContent.text = "Everyone survived another day.";
            return;
        }

        StringBuilder sb = new StringBuilder();
        foreach (var death in recentDeaths)
        {
            string categoryToSearch = "";
            switch (death.reason)
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
                    categoryToSearch = "Death_General";
                    break;
            }

            string deathAction = quacxiconSO.GetRandomLogFromCategory(categoryToSearch);
            sb.AppendLine($"{death.duckName} {deathAction}");
        }
        goodbyeFromLifeContent.text = sb.ToString();
    }

    public void UpdateArticles(List<ArticleEvent> candidateArticles)
    {
        if (candidateArticles == null || candidateArticles.Count == 0)
        {
            SetTopArticle("Quiet Day", "Nothing interesting happened. Our editors have taken the day off to swim in one of the seventeen identical lakes.");
            SetSecondArticle("Weather", "Still quacking sunny. Every day. Every day is sunny.");
            return;
        }

        var sortedArticles = candidateArticles.OrderByDescending(a => a.priority).ToList();

        SetTopArticle(sortedArticles[0].title, sortedArticles[0].content);

        if (sortedArticles.Count > 1)
        {
            SetSecondArticle(sortedArticles[1].title, sortedArticles[1].content);
        }
        else
        {
            SetSecondArticle("Ad", quacxiconSO.GetRandomLogFromCategory("Fluff_Ads"));
        }
    }

    //---------------------------------------------------------------------------------
    private void SetTopArticle(string title, string content)
    {
        if (topPriorityTitleText != null) topPriorityTitleText.text = title;
        if (topPriorityContentText != null) topPriorityContentText.text = content;
    }

    private void SetSecondArticle(string title, string content)
    {
        if (secondPriorityTitleText != null) secondPriorityTitleText.text = title;
        if (secondPriorityContentText != null) secondPriorityContentText.text = content;
    }
}