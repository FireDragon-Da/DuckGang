
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private int maxLines = 200;

    private readonly StringBuilder _builder = new StringBuilder();
    private int _lineCount;

    private void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponentInChildren<TMP_Text>(true);

        if (scrollRect == null)
            scrollRect = GetComponentInChildren<ScrollRect>(true) ?? GetComponentInParent<ScrollRect>();

        if (textComponent == null)
            Debug.LogWarning("TextBox: no TMP_Text assigned or found in children.");

        if (scrollRect == null)
            Debug.LogWarning("TextBox: no ScrollRect assigned or found in children/parents.");
    }

    //add a new line to the log
    public void AddLine(string line)
    {
        if (line == null)
            return;

        if (maxLines > 0 && _lineCount >= maxLines)
        {
            int firstNewline = _builder.ToString().IndexOf('\n');
            if (firstNewline >= 0)
            {
                _builder.Remove(0, firstNewline + 1);
                _lineCount--;
            }
        }

        _builder.AppendLine(line);
        _lineCount++;

        if (textComponent != null)
            textComponent.text = _builder.ToString();

        if (scrollRect != null)
            StartCoroutine(ScrollToBottomNextFrame());
    }

//replace the context
    public void SetText(string text)
    {
        _builder.Clear();
        if (!string.IsNullOrEmpty(text))
        {
            _builder.Append(text);
            //count lines
            _lineCount = text.Split('\n').Length;
        }
        else
        {
            _lineCount = 0;
        }

        if (textComponent != null)
            textComponent.text = _builder.ToString();

        if (scrollRect != null)
            StartCoroutine(ScrollToBottomNextFrame());
    }

//Clear the Log
    public void Clear()
    {
        _builder.Clear();
        _lineCount = 0;
        if (textComponent != null)
            textComponent.text = string.Empty;
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        // wait for end of frame so layout has been rebuilt
        yield return null;

        // Force rebuild to make sure content size is updated
        if (textComponent != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform as RectTransform);
        }

        if (scrollRect != null)
        {
            // If the ScrollRect's verticalNormalizedPosition is 1 = top, 0 = bottom
            // set to 0 to show the latest appended lines at the bottom
            scrollRect.verticalNormalizedPosition = 0f;

            // Force canvas update to apply the change immediately
            Canvas.ForceUpdateCanvases();
        }
    }
}
