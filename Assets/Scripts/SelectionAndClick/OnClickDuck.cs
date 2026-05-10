using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DuckStats))]
public class OnClickDuck : MonoBehaviour
{

    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private float sliderHeight = 50f;

    private DuckStats duckStats;
    private Camera mainCamera;

    private void Awake()
    {
        duckStats = GetComponent<DuckStats>();
        mainCamera = Camera.main;

    }

    private void OnMouseDown()
    {
        duckStats.ModifyHappiness(10);

        //DuckStatDisplay.reference.displayStats(this.gameObject.GetComponent<DuckNameGen>().CurrentDuckName, duckStats);
    }

    private void OnMouseUp()
    {

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DuckStatDisplay.reference.displayStats(this.gameObject.GetComponent<DuckNameGen>().CurrentDuckName, duckStats);
        }

    }
}