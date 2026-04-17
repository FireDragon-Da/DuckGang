using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform holderRect;
    RectTransform imageObject;
    Image image;

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite trashSprite;
    [SerializeField] Sprite grabSprite;

    public static MouseCursor reference;

    public enum CursorType
    {
        Normal,
        None,
        Trash,
        Grab,
    }

    CursorType curType;

    void Awake()
    {
        reference = this;
        imageObject = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        Cursor.visible = false;
        SetSprite(CursorType.Normal);
    }

    void Update()
    {
        holderRect.position = Input.mousePosition;
    }

    void OnDestroy() => Cursor.visible = true;
    void OnApplicationFocus(bool hasFocus) => Cursor.visible = !hasFocus;

    //This could prob be cleaner
    public void SetSprite(CursorType cursorType)
    {
        if (curType == CursorType.None)
        {
            image.enabled = true;
        }

        switch (cursorType)
        {
            case CursorType.Normal:
                image.sprite = normalSprite;
                imageObject.anchoredPosition = new(20,-20);
                break;
            case CursorType.None:
                image.enabled = false;
                break;
            case CursorType.Trash:
                image.sprite = trashSprite;
                imageObject.anchoredPosition = new(20,-20);
                break;
            case CursorType.Grab:
                image.sprite = grabSprite;
                imageObject.anchoredPosition = Vector3.zero;
                break;
                
        }

        curType = cursorType;
    }

}