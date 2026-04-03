using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{

    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = TimeManager.reference.curMonthTime;
    }
}
