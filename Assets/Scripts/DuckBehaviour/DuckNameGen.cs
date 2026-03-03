using UnityEngine;
using TMPro;
public class DuckNameGen : MonoBehaviour
{
    [SerializeField] private QuacxiconSO gameQuaxicon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private string duckName = "John Quackna";

    private void Start()
    {
        string duckName = gameQuaxicon.GetRandomLogFromCategory("DuckNames");
        if (nameText != null)
        {
            nameText.text = duckName;
        }
    }
}
