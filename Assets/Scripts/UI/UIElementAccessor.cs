using System.Collections.Generic;
using UnityEngine;

public class UIElementAccessor : MonoBehaviour
{
    public List<ObjectToggleAccessor> objects;
    public static UIElementAccessor reference;

    void Awake()
    {
        reference = this;
    }

    public void ForceAllOff()
    {
        foreach (ObjectToggleAccessor cur in objects)
        {
            cur.ForceOff();
        }
    }

}
