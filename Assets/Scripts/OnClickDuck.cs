using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DuckStats))]
public class OnClickDuck : MonoBehaviour
{
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
    }

    private void OnMouseDown()
    {
        ShowStatSliders();
    }

    private void ShowStatSliders()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 uiPosition = new Vector3(screenPos.x, screenPos.y, 0);

        float yOffset = 0;
        CreateStatSlider("Happiness", duckStats.Happiness, uiPosition, yOffset);
        yOffset -= sliderHeight + 10f;

        CreateStatSlider("Hunger", duckStats.Hunger, uiPosition, yOffset);
        yOffset -= sliderHeight + 10f;

        CreateStatSlider("Energy", duckStats.Energy, uiPosition, yOffset);
        yOffset -= sliderHeight + 10f;

        CreateStatSlider("Health", duckStats.Health, uiPosition, yOffset);
    }

    private void CreateStatSlider(string statName, int currentValue, Vector3 basePosition, float yOffset)
    {
        GameObject sliderGO = Instantiate(sliderPrefab, canvas.transform);

        RectTransform rectTransform = sliderGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(basePosition.x, basePosition.y + yOffset);

        Slider slider = sliderGO.GetComponent<Slider>();
        slider.maxValue = DuckStats.MaxStatValue;
        slider.value = currentValue;
        slider.interactable = false;

        Text labelText = sliderGO.GetComponentInChildren<Text>();
        if (labelText != null)
        {
            labelText.text = $"{statName}: {currentValue}/{DuckStats.MaxStatValue}";
        }

        Destroy(sliderGO, 3f);
    }
}
