using UnityEngine;

public class CrumbPlacer : MonoBehaviour
{

    [SerializeField] GameObject crumbPrefab;
    [SerializeField] float lureRange;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PlaceCrumb(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            print(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void PlaceCrumb(Vector2 location)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(location, lureRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Duck"))
            {
                DuckWalk cur = hit.GetComponent<DuckWalk>();
                if (cur != null)
                {
                    cur.Lure(location);
                }
            }
        }

        Instantiate(crumbPrefab, location, new());
    }

}
