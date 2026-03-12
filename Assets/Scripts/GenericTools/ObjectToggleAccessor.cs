using UnityEngine;

public class ObjectToggleAccessor : MonoBehaviour
{
    [SerializeField] GameObject target;
    public void Toggle()
    {
        target.SetActive(!target.activeSelf);
    }

    public void ForceOn()
    {
        target.SetActive(true);
    }

    public void ForceOff()
    {
        target.SetActive(false);
    }
}
