using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager reference;

    [SerializeField] float duckMonthLength;
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
    }

    void MonthPassed()
    {
        monthsPassed++;
        curMonthTime -= duckMonthLength;

        NewspaperController.reference.UpdateNewspaper(
            0,0,"",
            DuckSocietyManager.reference.newbornDuckNames,
            DuckSocietyManager.reference.recentDeaths,
            new()
        );

        DuckSocietyManager.reference.newbornDuckNames = new();
        DuckSocietyManager.reference.recentDeaths = new();
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
