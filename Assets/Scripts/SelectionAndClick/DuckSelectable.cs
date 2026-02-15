using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DuckSelectable : MonoBehaviour
{
    [SerializeField] private Color selectedColor = new Color(1f, 0.8f, 0.2f, 1f);
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isSelected;

    public bool IsSelected => isSelected;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Select()
    {
        if (isSelected) return;
        
        isSelected = true;
        spriteRenderer.color = selectedColor;
    }

    public void Deselect()
    {
        if (!isSelected) return;
        
        isSelected = false;
        spriteRenderer.color = originalColor;
    }
}
