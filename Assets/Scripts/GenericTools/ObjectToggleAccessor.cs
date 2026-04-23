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
        print("forcing on " +  target.name);
        target.SetActive(true);
    }

    public void ForceOff()
    {
        target.SetActive(false);
    }
}
