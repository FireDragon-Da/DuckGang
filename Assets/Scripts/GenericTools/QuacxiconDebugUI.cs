using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class QuacxiconDebugUI : MonoBehaviour
{
    [Header("References - Assign in Inspector")]
    [Tooltip("Drag your Quacxicon ScriptableObject asset here")]
    public ScriptableObject quacxiconData;

    [Header("UI Settings")]
    public KeyCode toggleKey = KeyCode.F1;
    public int fontSize = 16;

    private GameObject debugPanel;
    private Text debugText;
    private bool isVisible = false;

    void Start()
    {
        CreateSimpleDebugUI();
        LogDataStatus();

        ShowPanel();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (isVisible)
                HidePanel();
            else
                ShowPanel();
        }
    }

    void CreateSimpleDebugUI()
    {
        // Find or create canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("DebugCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }


        debugPanel = new GameObject("QuacxiconDebugPanel");
        debugPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = debugPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(0.4f, 0.6f);
        panelRect.offsetMin = new Vector2(10, 10);
        panelRect.offsetMax = new Vector2(-10, -10);

        Image panelBG = debugPanel.AddComponent<Image>();
        panelBG.color = new Color(0, 0, 0, 0.9f);

        GameObject textGO = new GameObject("DebugText");
        textGO.transform.SetParent(debugPanel.transform, false);

        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 10);
        textRect.offsetMax = new Vector2(-10, -10);

        debugText = textGO.AddComponent<Text>();
        debugText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        debugText.fontSize = fontSize;
        debugText.color = Color.white;
        debugText.alignment = TextAnchor.UpperLeft;

        debugPanel.SetActive(false);
    }

    void ShowPanel()
    {
        if (debugPanel == null) return;

        isVisible = true;
        debugPanel.SetActive(true);
        UpdateDebugText();
    }

    void HidePanel()
    {
        if (debugPanel == null) return;

        isVisible = false;
        debugPanel.SetActive(false);
    }

    void LogDataStatus()
    {
        Debug.Log("========== QuacxiconSO Debug ==========");

        if (quacxiconData == null)
        {
            Debug.LogError("[QuacxiconDebugUI] QuacxiconSO is NULL! Assign it in Inspector!");
            return;
        }

        var soType = quacxiconData.GetType();
        var categoriesField = soType.GetField("categories");

        if (categoriesField == null)
        {
            Debug.LogError("[QuacxiconDebugUI] Cannot find 'categories' field!");
            return;
        }

        var categoriesList = categoriesField.GetValue(quacxiconData) as System.Collections.IList;
        if (categoriesList == null)
        {
            Debug.LogError("[QuacxiconDebugUI] Categories is null!");
            return;
        }

        Debug.Log("[QuacxiconDebugUI] Asset name: " + quacxiconData.name);
        Debug.Log("[QuacxiconDebugUI] Categories: " + categoriesList.Count);

        int total = 0;
        for (int i = 0; i < categoriesList.Count; i++)
        {
            var cat = categoriesList[i];
            var catType = cat.GetType();

            var nameField = catType.GetField("categoryName");
            var listField = catType.GetField("contentList");

            string catName = (nameField != null) ? nameField.GetValue(cat) as string : "Unknown";
            var contentList = listField.GetValue(cat) as System.Collections.IList;
            int count = (contentList != null) ? contentList.Count : 0;
            total += count;

            string status = (count > 0) ? "OK" : "EMPTY";
            Debug.Log(string.Format("[{0}] {1}: {2} lines [{3}]", i, catName, count, status));

            if (count > 0 && contentList != null)
            {
                Debug.Log("  Sample: " + contentList[0]);
            }
        }

        Debug.Log("[QuacxiconDebugUI] TOTAL LINES: " + total);

        if (total == 0)
        {
            Debug.LogError("[QuacxiconDebugUI] *** NO DATA! Import and save in Editor! ***");
        }

        Debug.Log("=======================================");
    }

    void UpdateDebugText()
    {
        if (debugText == null) return;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== QUACXICON DATA STATUS ===");
        sb.AppendLine("Press " + toggleKey.ToString() + " to toggle");
        sb.AppendLine("");

        if (quacxiconData == null)
        {
            sb.AppendLine("ERROR: QuacxiconSO is NULL!");
            sb.AppendLine("Assign it in Inspector!");
            debugText.text = sb.ToString();
            return;
        }

        var soType = quacxiconData.GetType();
        var categoriesField = soType.GetField("categories");

        if (categoriesField == null)
        {
            sb.AppendLine("ERROR: Cannot access data!");
            debugText.text = sb.ToString();
            return;
        }

        var categoriesList = categoriesField.GetValue(quacxiconData) as System.Collections.IList;

        sb.AppendLine("Asset: " + quacxiconData.name);
        sb.AppendLine("Categories: " + (categoriesList != null ? categoriesList.Count.ToString() : "NULL"));
        sb.AppendLine("");

        if (categoriesList == null)
        {
            sb.AppendLine("ERROR: Categories list is null!");
            debugText.text = sb.ToString();
            return;
        }

        int total = 0;
        int empty = 0;

        for (int i = 0; i < categoriesList.Count; i++)
        {
            var cat = categoriesList[i];
            var catType = cat.GetType();

            var nameField = catType.GetField("categoryName");
            var listField = catType.GetField("contentList");

            string catName = (nameField != null) ? nameField.GetValue(cat) as string : "Unknown";
            var contentList = listField.GetValue(cat) as System.Collections.IList;
            int count = (contentList != null) ? contentList.Count : 0;
            total += count;

            if (count == 0)
            {
                empty++;
                sb.AppendLine("[" + i + "] " + catName + ": EMPTY");
            }
            else
            {
                sb.AppendLine("[" + i + "] " + catName + ": " + count + " lines");
            }
        }

        sb.AppendLine("");
        sb.AppendLine("=== SUMMARY ===");
        sb.AppendLine("Total Lines: " + total);
        sb.AppendLine("Empty Categories: " + empty);
        sb.AppendLine("");

        if (total == 0)
        {
            sb.AppendLine("*** WARNING: NO DATA! ***");
            sb.AppendLine("1. Import in Editor");
            sb.AppendLine("2. Save the asset");
            sb.AppendLine("3. Rebuild");
        }
        else
        {
            sb.AppendLine("SUCCESS: Data loaded!");
        }

        debugText.text = sb.ToString();
    }
}
