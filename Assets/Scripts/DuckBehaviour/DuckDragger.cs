using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DuckDragger : MonoBehaviour
{

    DuckWalk curDuck;
    [SerializeField] float dragSpeed = 15f;
    [SerializeField] LayerMask nonDragThrough;

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !BuildingPlacer.reference.Using)
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

                Vector2 move = mousePos-curDuck.transform.position;

                RaycastHit2D hit = Physics2D.CircleCast((Vector2)curDuck.transform.position, 
                                            0.5f, move.normalized, 
                                            move.magnitude, nonDragThrough);

                Vector2 target;
                
                if (hit)
                {
                    target = hit.centroid + hit.normal * 0.01f;
                }
                else
                {
                    target = mousePos;
                }

                curDuck.transform.position = Vector2.MoveTowards(curDuck.transform.position, target, dragSpeed*Time.deltaTime);

            }
        }



    }
}
