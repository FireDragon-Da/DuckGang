using System.Collections.Generic;
using UnityEngine;

public class DuckSelectionManager : MonoBehaviour
{
    [Header("Selection Circle")]
    [SerializeField] private float selectionRadius = 1.5f;
    [SerializeField] private Color circleColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private int circleSegments = 32;
    
    private Camera mainCamera;
    private LineRenderer circleRenderer;
    private HashSet<DuckSelectable> selectedDucks = new HashSet<DuckSelectable>();

    private void Awake()
    {
        mainCamera = Camera.main;
        SetupCircleRenderer();
    }

    private void SetupCircleRenderer()
    {
        GameObject circleObj = new GameObject("SelectionCircle");
        circleObj.transform.SetParent(transform);
        
        circleRenderer = circleObj.AddComponent<LineRenderer>();
        circleRenderer.useWorldSpace = true;
        circleRenderer.loop = true;
        circleRenderer.positionCount = circleSegments;
        circleRenderer.startWidth = 0.05f;
        circleRenderer.endWidth = 0.05f;
        circleRenderer.sortingOrder = 100;

        // we can change this later, for now we just want a simple circle
        circleRenderer.material = new Material(Shader.Find("Sprites/Default"));
        circleRenderer.startColor = circleColor;
        circleRenderer.endColor = circleColor;
    }

    private void Update()
    {
        UpdateCirclePosition();
        
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void UpdateCirclePosition()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        
        // Draw circle at mouse position
        for (int i = 0; i < circleSegments; i++)
        {
            float angle = (float)i / circleSegments * 2f * Mathf.PI;
            float x = mouseWorldPos.x + Mathf.Cos(angle) * selectionRadius;
            float y = mouseWorldPos.y + Mathf.Sin(angle) * selectionRadius;
            circleRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    private void HandleClick()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        
        // use raycast to check the duck
        RaycastHit2D directHit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        bool clickedOnDuck = directHit.collider != null && directHit.collider.CompareTag("Duck");
        
        // collab with direct click
        if (clickedOnDuck)
        {
            return;
        }

        // deselect all ducks 
        DeselectAll();

        // selection circle check
        Collider2D[] hits = Physics2D.OverlapCircleAll(mouseWorldPos, selectionRadius);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Duck"))
            {
                DuckSelectable selectable = hit.GetComponent<DuckSelectable>();
                if (selectable != null)
                {
                    selectable.Select();
                    selectedDucks.Add(selectable);
                }
            }
        }
    }

    private void DeselectAll()
    {
        foreach (DuckSelectable duck in selectedDucks)
        {
            if (duck != null)
            {
                duck.Deselect();
            }
        }
        selectedDucks.Clear();
    }

    public HashSet<DuckSelectable> GetSelectedDucks()
    {
        return selectedDucks;
    }
}
