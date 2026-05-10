using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ArticlePriority
{
    Fluff = 0,
    NewBuilding = 1,
    MentalHealthCrisis = 2,
    Starvation = 3,
    SocialFormation = 4,
    Required = 5
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

            case "Required":
                priority = ArticlePriority.Required;
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

    [Header("Life & Death List Limits")]
    [Tooltip("When the birth or death count is greater than this, only the first few lines are shown and the rest are summarized.")]
    [SerializeField] private int lifeDeathOverflowThreshold = 50;
    [Tooltip("How many entries to show when the list overflows the threshold.")]
    [SerializeField] private int lifeDeathMaxShownWhenOverflow = 10;
    [SerializeField] private string birthOverflowSuffixFormat = "and {0} ducks were born";
    [SerializeField] private string deathOverflowSuffixFormat = "and {0} ducks died";

    [Header("UI References - Articles")]
    [SerializeField] private TMP_Text topPriorityTitleText;
    [SerializeField] private TMP_Text topPriorityContentText;
    [SerializeField] private TMP_Text secondPriorityTitleText;
    [SerializeField] private TMP_Text secondPriorityContentText;

    [Header("UI References - Duck Photo")]
    [SerializeField] private Image duckPhotoImage1;
    [SerializeField] private Image duckPhotoImage2;

    // Static storage for captured photos - populated by TimeManager, persists across newspaper open/close
    public static Sprite LatestPhoto1;
    public static Sprite LatestPhoto2;
    public static int latestPhotoIndex = 0;

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

        //current stack frame
        UpdateQuote(currentHappiness, currentHunger);
        UpdateChangeInQuackland(currentQuacklandChange);
        UpdateLifeSection(newbornDuckNames);
        UpdateDeathSection(recentDeaths);
        UpdateArticles(currentEvents);
        RefreshDuckPhotos();
    }

    public void UpdateQuote(float happiness, float hunger)
    {
        float averageStat = (happiness + hunger) / 2f;

        int quoteLevel = Mathf.Clamp(Mathf.FloorToInt(averageStat / 10f), 0, 9);

        string quoteStr = quacxiconSO.GetSpecificLogFromCategory("Quotes", quoteLevel);

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

        int total = newbornDuckNames.Count;
        bool overflow = total > lifeDeathOverflowThreshold;
        int showCount = overflow ? Mathf.Min(lifeDeathMaxShownWhenOverflow, total) : total;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < showCount; i++)
        {
            string birthAction = quacxiconSO.GetRandomLogFromCategory("BirthActions");
            sb.AppendLine($"{newbornDuckNames[i]} {birthAction}");
        }

        if (overflow)
        {
            int remainder = total - showCount;
            sb.AppendLine(string.Format(birthOverflowSuffixFormat, remainder));
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

        int total = recentDeaths.Count;
        bool overflow = total > lifeDeathOverflowThreshold;
        int showCount = overflow ? Mathf.Min(lifeDeathMaxShownWhenOverflow, total) : total;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < showCount; i++)
        {
            var death = recentDeaths[i];
            string categoryToSearch = GetDeathLogCategory(death.reason);
            string deathAction = quacxiconSO.GetRandomLogFromCategory(categoryToSearch);
            sb.AppendLine($"{death.duckName} {deathAction}");
        }

        if (overflow)
        {
            int remainder = total - showCount;
            sb.AppendLine(string.Format(deathOverflowSuffixFormat, remainder));
        }

        goodbyeFromLifeContent.text = sb.ToString();
    }

    private static string GetDeathLogCategory(DeathReason reason)
    {
        switch (reason)
        {
            case DeathReason.OldAge:
                return "Death_OldAge";
            case DeathReason.Starvation:
                return "Death_Starvation";
            case DeathReason.Disappeared:
                return "Death_Disappeared";
            case DeathReason.Suicide:
                return "Death_Suicide";
            default:
                return "Death_General";
        }
    }

    public void UpdateArticles(List<ArticleEvent> candidateArticles)
    {
        print("NC: updating articles");
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

    //---------------------------------------------------------------------------------
    // Duck Photo Display
    //---------------------------------------------------------------------------------

    /// <summary>
    /// Reads the latest captured sprites from DuckPhotoCaptureManager and applies them to the UI images.
    /// Called on newspaper open and whenever a new capture arrives while the newspaper is open.
    /// </summary>
    public void RefreshDuckPhotos()
    {
        if (duckPhotoImage1 != null && LatestPhoto1 != null)
        {
            duckPhotoImage1.sprite = LatestPhoto1;
        }

        if (duckPhotoImage2 != null && LatestPhoto2 != null)
        {
            duckPhotoImage2.sprite = LatestPhoto2;
        }
    }

    public void PlayNewsPaperSound()
    {
        if (gameObject.activeSelf)
        {
            SoundSystem.instance.PlaySound("open-news");

        }
        else {
            SoundSystem.instance.PlaySound("close-news");

        }
    }
}