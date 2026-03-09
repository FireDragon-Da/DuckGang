using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSpeakManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuacxiconSO quacxiconSO;
    [SerializeField] private TextBox targetTextBox;
    [SerializeField] private Camera mainCamera;

    [Header("Settings")]
    [SerializeField] private float checkInterval = 30f;
    [SerializeField, Range(0f, 1f)] private float speakProbability = 0.5f;
    [SerializeField] private string speakCategoryName = "DuckThoughts";

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        StartCoroutine(DuckSpeakRoutine());
    }

    private IEnumerator DuckSpeakRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (Random.value <= speakProbability)
            {
                TriggerDuckSpeak();
            }
        }
    }

    private void TriggerDuckSpeak()
    {
        DuckNameGen[] allDucks = FindObjectsOfType<DuckNameGen>();
        List<DuckNameGen> visibleDucks = new List<DuckNameGen>();

        foreach (var duck in allDucks)
        {
            Vector3 viewPos = mainCamera.WorldToViewportPoint(duck.transform.position);

            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                visibleDucks.Add(duck);
            }
        }

        if (visibleDucks.Count == 0) return;

        DuckNameGen chosenDuck = visibleDucks[Random.Range(0, visibleDucks.Count)];

        string dialogLine = quacxiconSO.GetRandomLogFromCategory(speakCategoryName);
        if (string.IsNullOrEmpty(dialogLine)) return;

        string formattedMessage = $"<color=yellow>{chosenDuck.CurrentDuckName}: {dialogLine}</color>";

        if (targetTextBox != null)
        {
            targetTextBox.AddLine(formattedMessage);
        }
    }
}