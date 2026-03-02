using UnityEngine;
using UnityEngine.UI;

public class TestTBOX : MonoBehaviour
{
    public TextBox textBox;
    public Button button;
    public string quack;
    public int counter = 0;
    private void Awake()
    {
      quack = "Quack! \n";
    }
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);

    }

    private void OnButtonClick()
    {
        counter++;
        quack = "Quack! " + counter + "\n";
        if (textBox != null)
        {
            textBox.AddLine(quack);
        }
    }
}