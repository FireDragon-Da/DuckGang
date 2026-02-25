using UnityEngine;

public class DemoManager : MonoBehaviour
{
    [SerializeField] MapManager mapGen;
    [SerializeField] GameObject duckPrefab;
    [SerializeField] CrumbPlacer crumbPlacer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(mapGen.GenMap(result =>
        {
            if (result)
            {
                DuckSpawn();
                crumbPlacer.Activate();
            }            
        }));
    }

    // Update is called once per frame
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
