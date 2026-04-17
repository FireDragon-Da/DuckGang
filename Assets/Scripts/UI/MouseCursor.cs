using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    [Header("References")]
    RectTransform cursorRect;
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
        cursorRect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        curType = CursorType.Normal;
        Cursor.visible = false;
        image.sprite = normalSprite;
    }

    void Update()
    {
        cursorRect.position = Input.mousePosition;
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
                break;
            case CursorType.None:
                image.enabled = false;
                break;
            case CursorType.Trash:
                image.sprite = trashSprite;
                break;
            case CursorType.Grab:
                image.sprite = grabSprite;
                break;
                
        }

        curType = cursorType;
    }

}