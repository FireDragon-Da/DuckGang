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
        gameObject.SetActive(true);
    }

    public void ForceOff()
    {
        gameObject.SetActive(false);
    }
}
