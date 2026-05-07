using System.Collections;
using System.Collections.Generic;
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
    private readonly List<string> _messages = new List<string>();

    public static TextBox reference;

    private void Awake()
    {
        reference = this;

        if (textComponent == null)
            textComponent = GetComponentInChildren<TMP_Text>(true);

        if (scrollRect == null)
            scrollRect = GetComponentInChildren<ScrollRect>(true) ?? GetComponentInParent<ScrollRect>();

        if (textComponent == null)
            Debug.LogWarning("TBOX: No TMP_Text assigned or found in children.");

        if (scrollRect == null)
            Debug.LogWarning("TBOX: No ScrollRect assigned or found in children/parents.");

        if (scrollRect != null && textComponent != null)
        {
            if (scrollRect.content == null)
            {
                scrollRect.content = textComponent.transform.parent as RectTransform;
            }

            var viewport = scrollRect.viewport;
            if (viewport != null)
            {
                  var mask = viewport.GetComponent<Mask>();
                if (mask != null)
                {
                    DestroyImmediate(mask);
                    viewport.gameObject.AddComponent<RectMask2D>();
                }

                var viewportImage = viewport.GetComponent<Image>();
                if (viewportImage != null)
                {
                    // Make viewport interactable for scrolling and set a readable, non-bright
                    // semi-opaque background so text is easy to read.
                    viewportImage.raycastTarget = true;
                    // Soft dark background (not bright)
                    viewportImage.color = new Color(0.08f, 0.08f, 0.10f, 0.9f);
                }
            }

                 scrollRect.vertical = true;
            scrollRect.horizontal = false;
            scrollRect.scrollSensitivity = 20f;
        }
        if (textComponent != null)
        {
            var contentsizeFitter = textComponent.gameObject.GetComponent<ContentSizeFitter>();
            contentsizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentsizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            textComponent.enableWordWrapping = true;
            textComponent.overflowMode = TextOverflowModes.Overflow;

            textComponent.alignment = TextAlignmentOptions.TopLeft;
            // Ensure text color is readable against the chosen dark viewport background.
            // Use an off-white/soft color (not a harsh bright white).
            textComponent.color = new Color(0.95f, 0.95f, 0.90f, 1f);
        }

        gameObject.SetActive(false);
    }


    public void AddLine(string line)
    {
        if (line == null)
            return;


        _messages.Insert(0, line);

        if (maxLines > 0)
        {
            while (_messages.Count > maxLines)
            {
                _messages.RemoveAt(_messages.Count - 1);
            }
        }

    
        _builder.Clear();
        for (int i = 0; i < _messages.Count; i++)
        {
            _builder.AppendLine(_messages[i]);
        }

        if (textComponent != null)
            textComponent.text = _builder.ToString();

        if (scrollRect.isActiveAndEnabled)
            StartCoroutine(ScrollToTopNextFrame());
    }

    public void SetText(string text)
    {
        _messages.Clear();

        if (!string.IsNullOrEmpty(text))
        {

            var lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                _messages.Add(lines[i]);
            }

            if (maxLines > 0 && _messages.Count > maxLines)
            {
                _messages.RemoveRange(maxLines, _messages.Count - maxLines);
            }
        }


        _builder.Clear();
        for (int i = 0; i < _messages.Count; i++)
        {
            _builder.AppendLine(_messages[i]);
        }

        if (textComponent != null)
            textComponent.text = _builder.ToString();

        if (scrollRect != null)
            StartCoroutine(ScrollToTopNextFrame());
    }

    public void Clear()
    {
        _messages.Clear();
        _builder.Clear();
        if (textComponent != null)
            textComponent.text = string.Empty;
    }

    private IEnumerator ScrollToTopNextFrame()
    {

        yield return new WaitForEndOfFrame();


        if (scrollRect == null || textComponent == null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        }
        else if (textComponent != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform as RectTransform);
        }

        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;

            Canvas.ForceUpdateCanvases();
        }
    }
}
