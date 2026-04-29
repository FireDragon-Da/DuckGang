using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialActivateAccessor : MonoBehaviour
{
    [SerializeField] Tutorials target;

    public void ClickAccessor()
    {
        TutorialLines.reference.TryActivate(target);
    }
}
