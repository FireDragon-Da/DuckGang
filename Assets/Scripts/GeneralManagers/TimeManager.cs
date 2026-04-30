using System.Collections;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager reference;

    [SerializeField] int monthsPerMeeting = 5;
    [SerializeField] float duckMonthLength;

    [Header("Duck Photo Capture")]
    [SerializeField] private float photoCaptureInterval = 60f;
    [SerializeField] private int photoWidth = 512;
    [SerializeField] private int photoHeight = 512;
    [SerializeField] private float photoCaptureZoom = 3f;

    private float photoTimer;

    DuckSocietyManager dsm;
    ArticleCreator ac;

    int monthsPassed;
    public float curMonthTime;

    int pauses;

    public int GetPauseCount()
    {
        return pauses;
    }

    void Awake()
    {
        reference = this;
        pauses = 0;
        Time.timeScale = 1;
        photoTimer = photoCaptureInterval;
    }

    void OnDestroy()
    {
        if (reference == this)
        {
            reference = null;
        }
    }

    private void Start()
    {
        dsm = DuckSocietyManager.reference;
        ac = ArticleCreator.reference;
        if (dsm == null) print("dsm null");
        if (ac == null) print("ac null");
        dsm.articles.Add(ac.namedArticles["newMayorArticle"]);

        // Capture two photos immediately at game start so both image slots are filled
        StartCoroutine(CaptureInitialPhotos());
    }

    private IEnumerator CaptureInitialPhotos()
    {
        // Small delay to let the scene fully initialize before the first capture
        yield return null;
        CaptureDuckPhoto();

        // Capture the second slot right after so both images are populated from the start
        yield return null;
        CaptureDuckPhoto();
    }

    void Update()
    {
        curMonthTime += Time.deltaTime;

        photoTimer -= Time.deltaTime;
        if (photoTimer <= 0f)
        {
            photoTimer = photoCaptureInterval;
            CaptureDuckPhoto();
        }

        if (curMonthTime >= duckMonthLength)
        {
            MonthPassed();
        }
    }

    void MonthPassed()
    {
        monthsPassed++;
        curMonthTime -= duckMonthLength;

        //Moved meeting trigger to before newpaper update
        if (monthsPassed % monthsPerMeeting == 0)
        {
            UpgradeMeetingManager.reference.StartMeeting();
        }

        //add two random fluff articles to article list in case nothing else happened
        dsm.articles.Add(ac.fluffArticles[Random.Range(0, ac.fluffArticles.Count)]);
        dsm.articles.Add(ac.fluffArticles[Random.Range(0, ac.fluffArticles.Count)]);

        //calculate average happiness & hunger
        int happiness = 0;
        int hunger = 0;
        foreach(GameObject duck in PublicInfo.reference.duckList)
        {
            happiness += duck.GetComponent<DuckStats>().Happiness;
            hunger += duck.GetComponent<DuckStats>().Hunger;
        }

        happiness /= PublicInfo.reference.duckList.Count;
        hunger /= PublicInfo.reference.duckList.Count;

        NewspaperController.reference.UpdateNewspaper(
            hunger,happiness,"",
            dsm.newbornDuckNames,
            dsm.recentDeaths,
            dsm.articles
        );

        dsm.newbornDuckNames = new();
        dsm.recentDeaths = new();
        dsm.articles = new();
    }

    private void CaptureDuckPhoto()
    {
        GameObject[] allDucks = GameObject.FindGameObjectsWithTag("Duck");
        if (allDucks.Length == 0)
        {
            return;
        }

        GameObject randomDuck = allDucks[Random.Range(0, allDucks.Length)];
        StartCoroutine(CaptureScreenshotCoroutine(randomDuck));
    }

    private IEnumerator CaptureScreenshotCoroutine(GameObject targetDuck)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            yield break;
        }

        Vector3 originalPosition = cam.transform.position;
        float originalSize = cam.orthographicSize;

        Vector3 duckPosition = targetDuck.transform.position;
        cam.transform.position = new Vector3(duckPosition.x, duckPosition.y, cam.transform.position.z);
        cam.orthographicSize = photoCaptureZoom;

        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(photoWidth, photoHeight, 24);
        RenderTexture previousRT = RenderTexture.active;
        cam.targetTexture = renderTexture;
        cam.Render();

        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
        screenshot.Apply();

        cam.targetTexture = null;
        RenderTexture.active = previousRT;
        Destroy(renderTexture);

        cam.transform.position = originalPosition;
        cam.orthographicSize = originalSize;

        Sprite photoSprite = Sprite.Create(
            screenshot,
            new Rect(0, 0, screenshot.width, screenshot.height),
            new Vector2(0.5f, 0.5f)
        );

        if (NewspaperController.latestPhotoIndex == 0)
        {
            NewspaperController.LatestPhoto1 = photoSprite;
            NewspaperController.latestPhotoIndex = 1;
        }
        else
        {
            NewspaperController.LatestPhoto2 = photoSprite;
            NewspaperController.latestPhotoIndex = 0;
        }

        NewspaperController.reference.RefreshDuckPhotos();
    }

    public void AddPause()
    {
        pauses++;
        Time.timeScale = 0;
    }

    public void RemovePause()
    {
        pauses--;
        if (pauses < 0)
        {
            pauses = 0;
        }
        if (pauses == 0)
        {
            Time.timeScale = GameMenu.reference.Speed;
        }
    }

}
