using UnityEngine;

public class Demo2Manager : MonoBehaviour
{
    [SerializeField] GameObject duckPrefab;
    [SerializeField] CrumbPlacer crumbPlacer;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void DuckSpawn()
    {
        print("dsad");
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Instantiate(duckPrefab, new(i,j), new Quaternion());
            }
        }
    }

}
