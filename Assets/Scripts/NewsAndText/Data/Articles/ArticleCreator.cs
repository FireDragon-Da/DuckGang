using UnityEngine;

public class ArticleCreator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScriptableObject newArticle = ScriptableObject.CreateInstance(typeof(Article));
        ((Article)newArticle).title = "Apocolypse!!";

        //Article article = CreateAssetMenuAttribute("testArticle.asset", "agh", "agh");

        UnityEditor.AssetDatabase.CreateAsset(newArticle, "Assets/Scripts/NewsAndText/Data/Articles.asset");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
