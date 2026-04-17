using UnityEngine;
public enum DuckActionType
{
    None,
    Build,
    Water,
    Harvest
}

public class DuckActionIndicator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    
    private DuckActionType currentAction = DuckActionType.None;

    public void SetAction(DuckActionType action)
    {
        if (currentAction == action) return;

        currentAction = action;

        if (action == DuckActionType.None)
        {
            spriteRenderer.enabled = false;
            animator.Play("Idle");
            return;
        }

        spriteRenderer.enabled = true;

        switch (action)
        {
            
            case DuckActionType.Build:
                animator.Play("Build");
                break;
            case DuckActionType.Water:
                animator.Play("Water");
                break;
            case DuckActionType.Harvest:
                animator.Play("Harvest");
                break;
        }
    }

    public void ClearAction()
    {
        SetAction(DuckActionType.None);
    }
}
