using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalHungerDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private TextMeshProUGUI averageHungerText;

    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.5f;
    private float timer;

    private void Start()
    {
        if (hungerSlider != null)
        {
            hungerSlider.maxValue = DuckStats.MaxStatValue;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = updateInterval;
            CalculateAndDisplayAverageHunger();
        }
    }

    private void CalculateAndDisplayAverageHunger()
    {
        int duckCount = PublicInfo.reference.duckList.Count;

        if (duckCount == 0)
        {
            if (averageHungerText != null)
            {
                averageHungerText.text = "Average Hunger: 0/100 (No Ducks)";
            }
            if (hungerSlider != null)
            {
                hungerSlider.value = 0;
            }
            return;
        }

        int totalHunger = 0;
        foreach (GameObject duck in PublicInfo.reference.duckList)
        {
            totalHunger += duck.GetComponent<DuckStats>().Hunger;
        }

        int averageHunger = totalHunger / duckCount;

        if (averageHungerText != null)
        {
            averageHungerText.text = $"Average Hunger: {averageHunger}/100";
        }

        if (hungerSlider != null)
        {
            hungerSlider.value = averageHunger;
        }
    }
}