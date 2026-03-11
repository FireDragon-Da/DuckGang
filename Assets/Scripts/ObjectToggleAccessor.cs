using UnityEngine;

public class ObjectToggleAccessor : MonoBehaviour
{
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
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
