using UnityEngine.UI;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image fillbar;

    void Start()
    {
        fillbar.fillAmount = 0;
    }

    public void ChangeFill(float amount)
    {
        fillbar.fillAmount = amount;
    }

    public void ShowBar()
    {
        gameObject.SetActive(true);
    }

    public void HideBar()
    {
        gameObject.SetActive(false);
    }
}
