using System.Collections.Generic;
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

    public PopupMessageData(string textContent)
    {
        this.textContent = textContent;
        targetPosition = Vector2.zero;
    }
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public bool active = false;

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

        if (!active)
        {
            currentMessages = messages;
            currentIndex = 0;
            active = true;

            if (TimeManager.reference != null)
            {
                TimeManager.reference.AddPause();
            }
        }
        else
        {
            foreach (PopupMessageData message in messages)
            {
                currentMessages.Add(message);
            }
        }

        popupContainer.SetActive(true);
        DisplayCurrentMessage();
    }

    private void OnNextMessageClicked()
    {
        currentIndex++;

        //print("Message clicked! Displaying message " + currentIndex + "/" + currentMessages.Count);

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
        TimeManager.reference.RemovePause();
        active = false;
        currentMessages.Clear();
    }
}