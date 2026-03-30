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

        // Validate references
        Debug.Log("[DuckSpeakManager] Starting DuckSpeakManager");
        
        if (quacxiconSO == null)
        {
            Debug.LogError("[DuckSpeakManager] ? QuacxiconSO is NULL! Assign it in the inspector!");
        }
        else
        {
            Debug.Log($"[DuckSpeakManager] ? QuacxiconSO assigned: {quacxiconSO.name}");
            Debug.Log($"[DuckSpeakManager] Will use category: {speakCategoryName}");
        }
        
        if (targetTextBox == null)
        {
            Debug.LogWarning("[DuckSpeakManager] ?? TargetTextBox is NULL! Duck speech won't be displayed!");
        }
        else
        {
            Debug.Log($"[DuckSpeakManager] ? TextBox assigned: {targetTextBox.name}");
        }

        StartCoroutine(DuckSpeakRoutine());
    }

    private IEnumerator DuckSpeakRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            Debug.Log($"[DuckSpeakManager] Check interval reached. Probability: {speakProbability}");
            
            if (Random.value <= speakProbability)
            {
                TriggerDuckSpeak();
            }
            else
            {
                Debug.Log("[DuckSpeakManager] Probability check failed, not triggering speech");
            }
        }
    }

    private void TriggerDuckSpeak()
    {
        Debug.Log("[DuckSpeakManager] TriggerDuckSpeak called");
        
        DuckNameGen[] allDucks = FindObjectsOfType<DuckNameGen>();
        Debug.Log($"[DuckSpeakManager] Found {allDucks.Length} ducks in scene");
        
        List<DuckNameGen> visibleDucks = new List<DuckNameGen>();

        foreach (var duck in allDucks)
        {
            Vector3 viewPos = mainCamera.WorldToViewportPoint(duck.transform.position);

            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                visibleDucks.Add(duck);
            }
        }

        Debug.Log($"[DuckSpeakManager] Visible ducks: {visibleDucks.Count}");
        
        if (visibleDucks.Count == 0)
        {
            Debug.Log("[DuckSpeakManager] No visible ducks, aborting");
            return;
        }

        DuckNameGen chosenDuck = visibleDucks[Random.Range(0, visibleDucks.Count)];
        Debug.Log($"[DuckSpeakManager] Chosen duck: {chosenDuck.CurrentDuckName}");

        if (quacxiconSO == null)
        {
            Debug.LogError("[DuckSpeakManager] QuacxiconSO is null, cannot get dialog!");
            return;
        }

        Debug.Log($"[DuckSpeakManager] Requesting text from category: {speakCategoryName}");
        string dialogLine = quacxiconSO.GetRandomLogFromCategory(speakCategoryName);
        
        if (string.IsNullOrEmpty(dialogLine))
        {
            Debug.LogWarning($"[DuckSpeakManager] ? No dialog returned from category '{speakCategoryName}'!");
            return;
        }

        Debug.Log($"[DuckSpeakManager] ? Got dialog: {dialogLine}");

        string formattedMessage = $"<color=yellow>{chosenDuck.CurrentDuckName}: {dialogLine}</color>";
        Debug.Log($"[DuckSpeakManager] Formatted message: {formattedMessage}");

        if (targetTextBox != null)
        {
            targetTextBox.AddLine(formattedMessage);
            Debug.Log("[DuckSpeakManager] ? Message added to TextBox");
        }
        else
        {
            Debug.LogWarning("[DuckSpeakManager] ?? Cannot display message - TextBox is null!");
        }
    }
}