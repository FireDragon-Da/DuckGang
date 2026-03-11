using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DuckStats))]
public class OnClickDuck : MonoBehaviour
{

    DuckStatDisplay display;

    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private float sliderHeight = 50f;
    private Canvas canvas;

    private DuckStats duckStats;
    private Camera mainCamera;

    private void Awake()
    {
        duckStats = GetComponent<DuckStats>();
        mainCamera = Camera.main;
        canvas = FindObjectOfType<Canvas>();

        display = GameObject.Find("GameManager").GetComponentInChildren<DuckStatDisplay>();
    }

    private void OnMouseDown()
    {
        display.displayStats(this.gameObject.GetComponent<DuckNameGen>().CurrentDuckName, duckStats.Age, duckStats.Hunger, duckStats.Happiness);
        //ShowStatSliders();
        Debug.Log("DUCK STATS\n" + this.gameObject.GetComponent<DuckNameGen>().CurrentDuckName + ": " +
            "\nHunger: " + duckStats.Hunger + 
            "\nHappiness: " + duckStats.Happiness +
            "\nAge?: " + duckStats.Age);
    }

    private void ShowStatSliders()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 uiPosition = new Vector3(0, 0, 0);

        float yOffset = 0;

        CreateStatSlider("Hunger", duckStats.Hunger, DuckStats.MaxStatValue, new Color(1f, 0.5f, 0f), uiPosition, yOffset);
        yOffset -= sliderHeight + 10f;

        CreateStatSlider("Age", duckStats.Age, duckStats.MaxAge, new Color(0.2f, 0.8f, 0.2f), uiPosition, yOffset);
    }

    private void CreateStatSlider(string statName, int currentValue, int maxValue, Color sliderColor, Vector3 basePosition, float yOffset)
    {
        GameObject sliderGO = Instantiate(sliderPrefab, canvas.transform);

        RectTransform rectTransform = sliderGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(basePosition.x, basePosition.y + yOffset);

        Slider slider = sliderGO.GetComponent<Slider>();
        slider.maxValue = maxValue;
        slider.value = currentValue;
        slider.interactable = false;

        if (slider.fillRect != null)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = sliderColor;
            }
        }

        Text labelText = sliderGO.GetComponentInChildren<Text>();
        if (labelText != null)
        {
            labelText.text = $"{statName}: {currentValue}/{maxValue}";
        }

        Destroy(sliderGO, 3f);
    }
}