using UnityEngine;

public class ForceBuilder : MonoBehaviour
{
    void Start()
    {
        GetComponent<Building>().Build();
    }
}
