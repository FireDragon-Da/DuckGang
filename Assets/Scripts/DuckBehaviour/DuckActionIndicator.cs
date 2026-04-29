using UnityEngine;
public enum DuckActionType
{
    None,
    Build,
    Water,
    Harvest,
    Sacrifice,
    Deconstruct,
    StrawCraft,
    FillDiningHall,
    Compost,
    WatchSacrifice,
    Playground,
    Invest,
    LayNest
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
            case DuckActionType.Sacrifice:
                animator.Play("Sacrifice");
                break;
            case DuckActionType.Deconstruct:
                animator.Play("Deconstruct");
                break;
            case DuckActionType.StrawCraft:
                animator.Play("StrawCraft");
                break;
            case DuckActionType.FillDiningHall:
                animator.Play("FillDiningHall");
                break;
            case DuckActionType.Compost:
                animator.Play("Compost");
                break;
            case DuckActionType.WatchSacrifice:
                animator.Play("WatchSacrifice");
                break;
            case DuckActionType.Playground:
                animator.Play("Playground");
                break;
            case DuckActionType.Invest:
                animator.Play("Invest");
                break;
            case DuckActionType.LayNest:
                animator.Play("LayNest");
                break;
        }
    }

    public void ClearAction()
    {
        SetAction(DuckActionType.None);
    }
}
