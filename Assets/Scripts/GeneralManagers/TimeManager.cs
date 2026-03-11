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

        //TODO make newspaper show up
    }

}
