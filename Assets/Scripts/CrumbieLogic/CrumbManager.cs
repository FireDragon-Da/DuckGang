using TMPro;
using UnityEngine;

public class CrumbManager : MonoBehaviour
{
    
    public static CrumbManager reference;
    public int crumbs;
    [SerializeField] TextMeshProUGUI crumbCount;
    [SerializeField] CrumbiePopup crumbiePopupPrefab;


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
        PublicInfo.reference.crumbieEverCollected += amount;
        UpdateCrumbCount();
    }

    public bool ConsumeCrumbs(int amount)
    {
        if (crumbs >= amount)
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

    public void SpawnCrumbiePopupIncrease(Vector3 worldPosition, int amount)
    {
        CrumbiePopup popup = Instantiate(crumbiePopupPrefab, worldPosition, Quaternion.identity);
        
            popup.Setup(amount);

       
     
    }

    public void SpawnCrumbiePopupDecrease(Vector3 worldPosition, int amount)
    {
        CrumbiePopup popup = Instantiate(crumbiePopupPrefab, worldPosition, Quaternion.identity);

        popup.Setdown(amount);



    }

}
