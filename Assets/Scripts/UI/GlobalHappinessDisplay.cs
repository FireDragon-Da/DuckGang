using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalHappinessDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider happinessSlider;
    [SerializeField] private TextMeshProUGUI averageHappinessText;

    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.5f;
    private float timer;

    private void Start()
    {
        if (happinessSlider != null)
        {
            happinessSlider.maxValue = DuckStats.MaxStatValue;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = updateInterval;
            CalculateAndDisplayAverageHappiness();
        }
    }

    private void CalculateAndDisplayAverageHappiness()
    {
        DuckStats[] allDucks = FindObjectsOfType<DuckStats>();

        if (allDucks.Length == 0)
        {
            if (averageHappinessText != null)
            {
                averageHappinessText.text = "Average Happiness: 0/100 (No Ducks)";
            }
            if (happinessSlider != null)
            {
                happinessSlider.value = 0;
            }
            return;
        }

        int totalHappiness = 0;
        foreach (DuckStats duck in allDucks)
        {
            totalHappiness += duck.Happiness;
        }

        int averageHappiness = totalHappiness / allDucks.Length;

        if (averageHappinessText != null)
        {
            averageHappinessText.text = $"Average Happiness: {averageHappiness}/100";
        }

        if (happinessSlider != null)
        {
            happinessSlider.value = averageHappiness;
        }
    }
}