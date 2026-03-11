using TMPro;
using UnityEngine;

public class DuckStatDisplay : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nameAndAge;
    [SerializeField] TextMeshProUGUI hunger;
    [SerializeField] TextMeshProUGUI happiness;
    public int hello;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayStats(string name, int age, int hun, int hap)
    {
        gameObject.SetActive(true);
        nameAndAge.text = name + ", " + age;
        hunger.text = "hunger: " + hun + "/100";
        happiness.text = "happiness: " + hap + "/100";
    }
}
