using System.Collections.Generic;
using UnityEngine;

public class PopupTriggerTest : MonoBehaviour
{
    [SerializeField] private QuacxiconSO quacxiconSO;
    [SerializeField] private string targetCategory;
    [SerializeField] private List<Vector2> customPositions = new List<Vector2>();

    public bool triggerOnStart = false;

    [ContextMenu("Trigger Popup")]

    private void Start()
    {
        if (triggerOnStart)
        {
            TriggerMyPopup();
        }
    }
    public void TriggerMyPopup()
    {
        if (PopupManager.Instance == null || quacxiconSO == null) return;

        var category = quacxiconSO.categories.Find(c => c.categoryName == targetCategory);
        if (category == null || category.contentList.Count == 0) return;

        List<PopupMessageData> generatedMessages = new List<PopupMessageData>();

        for (int i = 0; i < category.contentList.Count; i++)
        {
            Vector2 pos = i < customPositions.Count ? customPositions[i] : Vector2.zero;

            generatedMessages.Add(new PopupMessageData
            {
                textContent = category.contentList[i],
                targetPosition = pos
            });
        }

        PopupManager.Instance.StartPopupSequence(generatedMessages);
    }
}