using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialActivateAccessor : MonoBehaviour
{
    [SerializeField] Tutorials target;
    [SerializeField] bool destroyOnUse;

    public void ClickAccessor()
    {
        TutorialLines.reference.TryActivate(target);
        if (destroyOnUse)
        {
            Destroy(gameObject);
        }
    }
}
