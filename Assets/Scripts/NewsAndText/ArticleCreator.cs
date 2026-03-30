using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

//HOW TO USE THIS SCRIPT
//this script should be attached to the gamemanager.
//drag the TXT_Articles file (should be in the same folder) into the textFile field in the editor

//the dictionary namedArticles contains all specific articles relating to game properties
    //these can be accessed by their keys, ie StarvationArticle or FarmArticle
//the list fluffArticles has all of the random articles that can be filled in in there are no named articles for the month
    //these can just be added randomly since they won't correspond to anything

public class ArticleCreator : MonoBehaviour
{
    public Dictionary<string, ArticleEvent> namedArticles = new Dictionary<string, ArticleEvent>();
    public List<ArticleEvent> fluffArticles = new List<ArticleEvent>();
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

        if (data[0].Trim() == "Fluff")
        {
            fluffArticles.Add(newArticle);
        }
        else
        {
            namedArticles.Add(data[0], newArticle);
        }
    }

}
