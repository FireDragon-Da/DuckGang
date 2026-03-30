using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogCategory
{
    public string categoryName; 
    
    // CRITICAL: This list MUST be serialized to persist data in builds
    // The TextAsset is only for importing - the actual data lives here
    [SerializeField]
    [TextArea(3, 10)] 
    public List<string> contentList = new List<string>();

    [Header("Import Settings")]
    public TextAsset sourceTxtFile;
    public string targetCategoryName;
    public bool clearExistingContent = true;
    
    [ContextMenu("Import Logs from Text File")]
    public void ImportFromSource()
    {
        if (sourceTxtFile == null) return;
        if (clearExistingContent)
            contentList.Clear();

        string[] lines = sourceTxtFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            if (!string.IsNullOrEmpty(trimmedLine))
            {
                contentList.Add(trimmedLine);
            }
        }
        
        Debug.Log($"[QuacxiconSO] Imported {contentList.Count} lines into category: {categoryName}");
    }
}


[CreateAssetMenu(fileName = "NewQuacxicon", menuName = "GameData/NewQuacxicon")]
public class QuacxiconSO : ScriptableObject
{
    // CRITICAL: categories list must be serialized for build persistence
    [SerializeField]
    public List<LogCategory> categories = new List<LogCategory>();

    // Runtime validation - logs on asset load
    private void OnEnable()
    {
        ValidateDataAtRuntime();
    }

    private void ValidateDataAtRuntime()
    {
        Debug.Log($"[QuacxiconSO Runtime] Loading asset: {name}");
        Debug.Log($"[QuacxiconSO Runtime] Categories count: {categories.Count}");
        
        int totalLines = 0;
        for (int i = 0; i < categories.Count; i++)
        {
            var category = categories[i];
            int lineCount = category.contentList != null ? category.contentList.Count : 0;
            totalLines += lineCount;
            
            if (lineCount == 0)
            {
                Debug.LogWarning($"[QuacxiconSO Runtime] Category '{category.categoryName}' is EMPTY!");
            }
            else
            {
                Debug.Log($"[QuacxiconSO Runtime] Category '{category.categoryName}': {lineCount} lines loaded");
            }
        }
        
        Debug.Log($"[QuacxiconSO Runtime] TOTAL: {totalLines} lines across all categories");
        
        if (totalLines == 0)
        {
            Debug.LogError($"[QuacxiconSO Runtime] ? NO DATA LOADED! Asset '{name}' is empty! Did you import and save the data in editor?");
        }
    }

    [ContextMenu("Import All Files into Categories")]
    public void ImportAll()
    {
        int count = 0;
        int totalLines = 0;
        
        foreach (var category in categories)
        {
            if (category.sourceTxtFile != null)
            {
                int beforeCount = category.contentList.Count;
                category.ImportFromSource();
                totalLines += category.contentList.Count;
                count++;
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"[QuacxiconSO] Import complete: {count} categories, {totalLines} total lines imported and SAVED to asset");
#endif
    }

    public string GetRandomLogFromCategory(string name)
    {
        Debug.Log($"[QuacxiconSO] GetRandomLogFromCategory called for: {name}");
        
        var category = categories.Find(c => c.categoryName == name);
        if (category != null && category.contentList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, category.contentList.Count);
            string result = category.contentList[randomIndex];
            Debug.Log($"[QuacxiconSO] Returning line {randomIndex}/{category.contentList.Count}: {result}");
            return result;
        }
        
        Debug.LogWarning($"[QuacxiconSO] Category '{name}' not found or empty! Available categories: {string.Join(", ", categories.ConvertAll(c => c.categoryName))}");
        return null;
    }

    public string GetSpecificLogFromCategory(string name, int order)
    {
        Debug.Log($"[QuacxiconSO] GetSpecificLogFromCategory called for: {name}, index: {order}");
        
        var category = categories.Find(c => c.categoryName == name);
        if (category != null && category.contentList.Count > 0 && order >= 0 && order < category.contentList.Count)
        {
            string result = category.contentList[order];
            Debug.Log($"[QuacxiconSO] Returning line {order}: {result}");
            return result;
        }
        
        Debug.LogWarning($"[QuacxiconSO] GetSpecificLogFromCategory failed for '{name}' index {order}");
        return null;
    }

    public int GetCategoryMaxIndex(string name)
    {
        var category = categories.Find(c => c.categoryName == name);
        if (category != null)
        {
            Debug.Log($"[QuacxiconSO] GetCategoryMaxIndex for '{name}': {category.contentList.Count}");
            return category.contentList.Count;
        }
        
        Debug.LogWarning($"[QuacxiconSO] GetCategoryMaxIndex failed for '{name}'");
        return 0;
    }
}