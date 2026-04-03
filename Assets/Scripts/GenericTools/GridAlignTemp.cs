using UnityEngine;

public class GridAlignTemp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float roundx = Mathf.Round(transform.localPosition.x * 2);
        float roundy = Mathf.Round(transform.localPosition.y * 2);

        print("I rounded " + transform.localPosition.x + " to " + roundx / 2);
        print("I rounded " + transform.localPosition.x + " to " + roundx / 2);

        transform.localPosition = new Vector3(roundx, roundy, 0);

        print("My position is " + transform.localPosition);
    }

}
