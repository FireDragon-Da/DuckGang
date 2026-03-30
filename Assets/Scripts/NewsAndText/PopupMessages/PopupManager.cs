using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct PopupMessageData
{
    [TextArea(2, 5)]
    public string textContent;
    public Vector2 targetPosition;
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField] private GameObject popupContainer;
    [SerializeField] private RectTransform popupRect;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button clickBackgroundButton;

    private List<PopupMessageData> currentMessages;
    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (popupContainer != null) popupContainer.SetActive(false);

        if (clickBackgroundButton != null)
        {
            clickBackgroundButton.onClick.AddListener(OnNextMessageClicked);
        }
    }

    public void StartPopupSequence(List<PopupMessageData> messages)
    {
        if (messages == null || messages.Count == 0) return;

        currentMessages = messages;
        currentIndex = 0;

        popupContainer.SetActive(true);
        DisplayCurrentMessage();
    }

    private void OnNextMessageClicked()
    {
        currentIndex++;

        if (currentIndex < currentMessages.Count)
        {
            DisplayCurrentMessage();
        }
        else
        {
            ClosePopup();
        }
    }

    private void DisplayCurrentMessage()
    {
        PopupMessageData currentData = currentMessages[currentIndex];

        if (messageText != null) messageText.text = currentData.textContent;
        if (popupRect != null) popupRect.anchoredPosition = currentData.targetPosition;
    }

    private void ClosePopup()
    {
        popupContainer.SetActive(false);
        currentMessages.Clear();
    }
}