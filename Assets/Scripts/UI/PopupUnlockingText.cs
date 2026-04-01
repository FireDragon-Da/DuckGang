using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PopupUnlockingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float moveSpeed = 50f;
    [SerializeField] float scalePop = 1.3f;
    [SerializeField] float popDuration = 0.2f;

    private RectTransform rect;
    private Vector3 originalScale;
    private float popTimer;
    private int clickCount = 0;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Setup(string message)
    {
        text.text = message;

        originalScale = transform.localScale;
        transform.localScale = originalScale * scalePop;
        popTimer = popDuration;
        clickCount = 0;
    }

    void Update()
    {
        //rect.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;

        if (popTimer > 0f)
        {
            popTimer -= Time.deltaTime;
            float t = 1f - (popTimer / popDuration);
            transform.localScale = Vector3.Lerp(originalScale * scalePop, originalScale, t);
        }
        else
        {
            transform.localScale = originalScale;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            clickCount++;

        }

        if (clickCount >= 2)
        {
            ClosePopup();
        }
    }

    void ClosePopup()
    {
        if (PopupUnlokcingTextManager.instance != null)
        {
            PopupUnlokcingTextManager.instance.OnPopupClosed(this);
        }

        Destroy(gameObject);
    }


}
