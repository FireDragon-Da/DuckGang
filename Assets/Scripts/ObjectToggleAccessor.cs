using UnityEngine;

public class ObjectToggleAccessor : MonoBehaviour
{
    [SerializeField] GameObject constructionUI;
    public void Toggle()
    {
        constructionUI.SetActive(!constructionUI.activeSelf);
    }

    public void ForceOn()
    {
        constructionUI.SetActive(true);
    }

    public void ForceOff()
    {
        constructionUI.SetActive(false);
    }
}
