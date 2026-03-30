using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float duckMonthLength;
    //HENRY - change DML back to 60 in editor
    int monthsPassed;
    float curMonthTime;

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
