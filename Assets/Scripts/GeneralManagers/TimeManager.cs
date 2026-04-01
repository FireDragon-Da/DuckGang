using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager reference;

    [SerializeField] float duckMonthLength;
    //HENRY - change DML back to 60 in editor
    int monthsPassed;
    float curMonthTime;

    int pauses;

    void Awake()
    {
        reference = this;
    }

    void Update()
    {
        curMonthTime += Time.deltaTime;
        //print(curMonthTime);
        if (curMonthTime >= duckMonthLength)
        {
            MonthPassed();
        }
        //HENRY
       // print(curMonthTime);
    }

    void MonthPassed()
    {
        monthsPassed++;
        curMonthTime -= duckMonthLength;

        //add two random fluff articles to article list in case nothing else happened
        DuckSocietyManager.reference.articles.Add(ArticleCreator.reference.fluffArticles[Random.Range(0, ArticleCreator.reference.fluffArticles.Count)]);
        DuckSocietyManager.reference.articles.Add(ArticleCreator.reference.fluffArticles[Random.Range(0, ArticleCreator.reference.fluffArticles.Count)]);

        NewspaperController.reference.UpdateNewspaper(
            0,0,"",
            DuckSocietyManager.reference.newbornDuckNames,
            DuckSocietyManager.reference.recentDeaths,
            DuckSocietyManager.reference.articles
        );

        DuckSocietyManager.reference.newbornDuckNames = new();
        DuckSocietyManager.reference.recentDeaths = new();
        DuckSocietyManager.reference.articles = new();
    }

    void AddPause()
    {
        pauses++;
        Time.timeScale = 0;
    }

    void RemovePause()
    {
        pauses--;
        if (pauses == 0)
        {
            Time.timeScale = 1;
        }
    }

}
