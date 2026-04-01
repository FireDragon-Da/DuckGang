using UnityEngine;
using System.Collections.Generic;


public class PopupUnlokcingTextManager : MonoBehaviour
{
    public static PopupUnlokcingTextManager instance;

    [SerializeField] GameObject popupPrefab;
    [SerializeField] RectTransform canvas;

    private Queue<string> messageQueue = new Queue<string>();
    private PopupUnlockingText currentPopup;

    void Awake()
    {
        instance = this;
    }

    public void ShowText(string message)
    {
        messageQueue.Enqueue(message);

        if (currentPopup == null)
        {
            ShowNextText();
        }
    }

    void ShowNextText()
    {
        if (messageQueue.Count == 0)
        {
            currentPopup = null;
            return;
        }

        string nextMessage = messageQueue.Dequeue();

        GameObject obj = Instantiate(popupPrefab, canvas);
        currentPopup = obj.GetComponent<PopupUnlockingText>();

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        currentPopup.Setup(nextMessage);
    }

    public void OnPopupClosed(PopupUnlockingText popup)
    {
        if (popup == currentPopup)
        {
            currentPopup = null;
            ShowNextText();
        }
    }
}
