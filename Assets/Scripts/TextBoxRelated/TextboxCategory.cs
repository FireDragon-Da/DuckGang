using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LogCategory
{
    public string categoryName; 
    [TextArea(3, 10)] 
    public List<string> contentList = new List<string>(); 
}


[CreateAssetMenu(fileName = "NewQuacxicon", menuName = "GameData/NewQuacxicon")]
public class QuacxiconSO : ScriptableObject
{
    public List<LogCategory> categories = new List<LogCategory>();

    public string GetRandomLogFromCategory(string name)
    {
        var category = categories.Find(c => c.categoryName == name);
        if (category != null && category.contentList.Count > 0)
        {
            int randomIndex = Random.Range(0, category.contentList.Count);
            return category.contentList[randomIndex];
        }
        return null;
    }
}