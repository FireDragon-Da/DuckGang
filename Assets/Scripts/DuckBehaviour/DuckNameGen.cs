using UnityEngine;
using TMPro;

public class DuckNameGen : MonoBehaviour
{
    [SerializeField] private QuacxiconSO gameQuaxicon;
    [SerializeField] private TMP_Text nameText;
    public string CurrentDuckName { get; private set; } = "John Quackna";

    private void Start()
    {
        CurrentDuckName = gameQuaxicon.GetRandomLogFromCategory("DuckNames");

        if (nameText != null)
        {
            nameText.text = CurrentDuckName;
        }
    }

}