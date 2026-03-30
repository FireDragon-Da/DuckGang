using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

//HOW TO USE THIS SCRIPT
//this script should be attached to the gamemanager.
//drag the TXT_Articles file (should be in the same folder) into the textFile field in the editor

public class ArticleCreator : MonoBehaviour
{
    public Dictionary<string, ArticleEvent> allArticles = new Dictionary<string, ArticleEvent>();
    public TextAsset textFile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (textFile == null)
        {
            print("No text file attached to articleCreator! articles will not work :(");
            return;
        }

        string[] articles = textFile.text.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string article in articles)
        {
            createArticle(article.Trim());
        }
    }

    public void createArticle(string fullArticle)
    {
        string[] data = fullArticle.Split(new[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);

        if (data.Length != 4 )
        {
            print("bad article! skipping...");
            return;
        }

        ArticleEvent newArticle = new ArticleEvent(data[1].Trim(), data[2].Trim(), data[3].Trim());

        allArticles.Add(data[0], newArticle);
    }

}
