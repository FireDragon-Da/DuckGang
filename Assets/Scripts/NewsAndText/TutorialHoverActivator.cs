using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialHoverActivator : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Tutorials target;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TutorialLines.reference.TryActivate(target);
    }
}
