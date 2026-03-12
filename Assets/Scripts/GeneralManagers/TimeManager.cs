using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float duckMonthLength;
    int monthsPassed;
    float curMonthTime;

    void Update()
    {
        curMonthTime += Time.deltaTime;
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

}
