using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager reference;

    [SerializeField] int monthsPerMeeting = 5;
    [SerializeField] float duckMonthLength;

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
        Debug.Log("[TimeManager] Awake - Reset pause counter to 0");
    }

    private void Start()
    {
        dsm = DuckSocietyManager.reference;
        ac = ArticleCreator.reference;
        if (dsm == null) print("dsm null");
        if (ac == null) print("ac null");
        dsm.articles.Add(ac.namedArticles["newMayorArticle"]);
    }

    void Update()
    {
        curMonthTime += Time.deltaTime;

        if (curMonthTime >= duckMonthLength)
        {
            MonthPassed();
            if (monthsPassed % monthsPerMeeting == 0)
            {
                MeetingManager.reference.StartMeeting();
            }
        }
        //HENRY
       // print(curMonthTime);
    }

    void MonthPassed()
    {
        monthsPassed++;
        curMonthTime -= duckMonthLength;

        //add two random fluff articles to article list in case nothing else happened
        dsm.articles.Add(ac.fluffArticles[Random.Range(0, ac.fluffArticles.Count)]);
        dsm.articles.Add(ac.fluffArticles[Random.Range(0, ac.fluffArticles.Count)]);

        NewspaperController.reference.UpdateNewspaper(
            0,0,"",
            dsm.newbornDuckNames,
            dsm.recentDeaths,
            dsm.articles
        );

        dsm.newbornDuckNames = new();
        dsm.recentDeaths = new();
        dsm.articles = new();
    }

    public void AddPause()
    {
        pauses++;
        Time.timeScale = 0;
        Debug.Log($"[TimeManager] AddPause called. Pauses: {pauses}, Time.timeScale: {Time.timeScale}");
    }

    public void RemovePause()
    {
        pauses--;
        if (pauses < 0)
        {
            Debug.LogWarning("TimeManager: Pause counter went negative, resetting to 0");
            pauses = 0;
        }
        if (pauses == 0)
        {
            Time.timeScale = GameMenu.reference.Speed;
            Debug.Log($"[TimeManager] RemovePause - resuming. Pauses: {pauses}, Time.timeScale: {Time.timeScale}, GameMenu.Speed: {GameMenu.reference.Speed}");
        }
        else
        {
            Debug.Log($"[TimeManager] RemovePause - still paused. Pauses: {pauses}, Time.timeScale: {Time.timeScale}");
        }
    }

}
