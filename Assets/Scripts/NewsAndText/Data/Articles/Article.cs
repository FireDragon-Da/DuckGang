using UnityEngine;

[CreateAssetMenu(fileName = "Article", menuName = "Scriptable Objects/Article")]
public class Article : ScriptableObject
{
    public string title;
    public string content;
    public ArticlePriority priority;
}
