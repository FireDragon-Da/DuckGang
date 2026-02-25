using TMPro;
using UnityEngine;

public class CrumbManager : MonoBehaviour
{
    
    public static CrumbManager reference;
    public int crumbs;
    [SerializeField] TextMeshProUGUI crumbCount;

    void Awake()
    {
        reference = this;
    }

    void Start()
    {
        UpdateCrumbCount();
    }

    public void GainCrumbs(int amount)
    {
        crumbs += amount;
        UpdateCrumbCount();
    }

    public bool ConsumeCrumbs(int amount)
    {
        if (crumbs > amount)
        {
            crumbs -= amount;
            UpdateCrumbCount();
            return true;
        }
        return false;
    }

    void UpdateCrumbCount()
    {
        crumbCount.text = crumbs.ToString();
    }

}
