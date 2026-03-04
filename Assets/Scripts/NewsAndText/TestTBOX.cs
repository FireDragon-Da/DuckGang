using UnityEngine;
using UnityEngine.UI;

public class TestTBOX : MonoBehaviour
{
    public TextBox textBox;
    public Button button;
    public string quack;
    public int counter = 0;
    public QuacxiconSO gameQuaxicon;
    public bool isQuackorderly = true;
    private void Awake()
    {
    }
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);

    }

    private void OnButtonClick()
    {
        if (isQuackorderly) { 
            if (counter >= gameQuaxicon.GetCategoryMaxIndex("Test"))
            {
                counter = 0;
            }
            quack = gameQuaxicon.GetSpecificLogFromCategory("Test", counter);
            counter++;
        }
        else
            quack = gameQuaxicon.GetRandomLogFromCategory("Test");
        if (textBox != null)
        {
            textBox.AddLine(quack);
        }
    }
}