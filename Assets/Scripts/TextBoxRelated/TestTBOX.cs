using UnityEngine;
using UnityEngine.UI;

public class TestTBOX : MonoBehaviour
{
    public TextBox textBox;
    public Button button;
    public string quack;
    public int counter = 0;
    public QuacxiconSO gameQuaxicon;
    private void Awake()
    {
    }
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);

    }

    private void OnButtonClick()
    {
        quack = gameQuaxicon.GetRandomLogFromCategory("Test");
        if (textBox != null)
        {
            textBox.AddLine(quack);
        }
    }
}