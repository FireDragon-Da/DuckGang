using System.Collections;
using System.Collections;
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

    [Header("UI References - Articles")]
    [SerializeField] private TMP_Text topPriorityTitleText;
    [SerializeField] private TMP_Text topPriorityContentText;
    [SerializeField] private TMP_Text secondPriorityTitleText;
    [SerializeField] private TMP_Text secondPriorityContentText;

    [Header("UI References - Duck Photo")]
    [SerializeField] private Image duckPhotoImage1;
    [SerializeField] private Image duckPhotoImage2;

    [Header("Photo Capture Settings")]
    [SerializeField] private float photoCaptureInterval = 60f;
    [SerializeField] private int photoWidth = 512;
    [SerializeField] private int photoHeight = 512;
    [SerializeField] private float photoCaptureZoom = 3f;

    private float photoTimer;
    private Camera mainCamera;
    private int currentPhotoIndex = 0;

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

        mainCamera = Camera.main;
        photoTimer = photoCaptureInterval;
    }

    void Update()
    {
        photoTimer -= Time.deltaTime;
        if (photoTimer <= 0f)
        {
            photoTimer = photoCaptureInterval;
            CaptureDuckPhoto();
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
    // Duck Photo Capture System
    //---------------------------------------------------------------------------------

    private void CaptureDuckPhoto()
    {
        GameObject randomDuck = GetRandomDuck();
        if (randomDuck == null)
        {
            Debug.Log("No ducks available for photo capture");
            return;
        }

        StartCoroutine(CaptureScreenshotCoroutine(randomDuck));
    }

    private GameObject GetRandomDuck()
    {
        GameObject[] allDucks = GameObject.FindGameObjectsWithTag("Duck");
        if (allDucks.Length == 0)
        {
            return null;
        }

        return allDucks[Random.Range(0, allDucks.Length)];
    }

    private IEnumerator CaptureScreenshotCoroutine(GameObject targetDuck)
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 originalPosition = mainCamera.transform.position;
        float originalSize = mainCamera.orthographicSize;

        Vector3 duckPosition = targetDuck.transform.position;
        mainCamera.transform.position = new Vector3(duckPosition.x, duckPosition.y, mainCamera.transform.position.z);
        mainCamera.orthographicSize = photoCaptureZoom;

        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(photoWidth, photoHeight, 24);
        RenderTexture currentRT = RenderTexture.active;
        mainCamera.targetTexture = renderTexture;

        mainCamera.Render();

        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
        screenshot.Apply();

        mainCamera.targetTexture = null;
        RenderTexture.active = currentRT;
        Destroy(renderTexture);

        mainCamera.transform.position = originalPosition;
        mainCamera.orthographicSize = originalSize;

        UpdateDuckPhoto(screenshot);
    }

    private void UpdateDuckPhoto(Texture2D photoTexture)
    {
        if (photoTexture == null)
        {
            return;
        }

        Sprite photoSprite = Sprite.Create(
            photoTexture,
            new Rect(0, 0, photoTexture.width, photoTexture.height),
            new Vector2(0.5f, 0.5f)
        );

        if (currentPhotoIndex == 0 && duckPhotoImage1 != null)
        {
            duckPhotoImage1.sprite = photoSprite;
            currentPhotoIndex = 1;
        }
        else if (currentPhotoIndex == 1 && duckPhotoImage2 != null)
        {
            duckPhotoImage2.sprite = photoSprite;
            currentPhotoIndex = 0;
        }
    }
}