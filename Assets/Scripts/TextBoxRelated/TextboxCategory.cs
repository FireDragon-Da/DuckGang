using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogCategory
{
    public string categoryName; 
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
    }

}


[CreateAssetMenu(fileName = "NewQuacxicon", menuName = "GameData/NewQuacxicon")]
public class QuacxiconSO : ScriptableObject
{
    public List<LogCategory> categories = new List<LogCategory>();

    [ContextMenu("Import All Files into Categories")]
    public void ImportAll()
    {
        int count = 0;
        foreach (var category in categories)
        {
            if (category.sourceTxtFile != null)
            {
                category.ImportFromSource();
                count++;
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }



    public string GetRandomLogFromCategory(string name)
    {
        var category = categories.Find(c => c.categoryName == name);
        if (category != null && category.contentList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, category.contentList.Count);
            return category.contentList[randomIndex];
        }
        return null;
    }

    public string GetSpecificLogFromCategory(string name, int order)
    {
        var category = categories.Find(c => c.categoryName == name);
        if (category != null && category.contentList.Count > 0 && order >= 0 && order < category.contentList.Count)
        {
            return category.contentList[order];
        }
        return null;
    }
}