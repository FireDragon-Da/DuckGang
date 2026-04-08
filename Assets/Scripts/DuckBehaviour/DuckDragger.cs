using UnityEngine;
using UnityEngine.EventSystems;

public class DuckDragger : MonoBehaviour
{

    DuckWalk curDuck;

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits;

            if (Input.GetMouseButtonDown(0))
            {
                hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.CompareTag("Duck"))
                    {
                        DuckWalk tempCurDuck = hit.collider.GetComponent<DuckWalk>();
                        if (tempCurDuck.CanBeGrabbed)
                        {
                            curDuck = tempCurDuck;
                            curDuck.beingDragged = true;
                        }
                    }
                }


            }
        }

        if (curDuck)
        {
            if (Input.GetMouseButtonUp(0))
            {
                curDuck.beingDragged = false;

                curDuck.Place();

                curDuck = null;
            }
            else
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                curDuck.transform.position = mousePos;
            }
        }



    }
}
