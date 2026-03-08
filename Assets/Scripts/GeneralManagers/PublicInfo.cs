using System.Collections.Generic;
using UnityEngine;

public class PublicInfo : MonoBehaviour
{
    public static PublicInfo reference;

    public List<GameObject> duckList = new();
    public List<Nest> nestList = new();

    void Awake()
    {
        if (reference == null)
        {
            reference = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    //Random functions just for making other scripts cleaner
    public bool AnyNestEmpty()
    {
        foreach (Nest cur in nestList)
        {
            if (cur.Empty)
            {
                return true;
            }
        }

        return false;
    }

}
