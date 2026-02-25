using UnityEngine;
using UnityEngine.UI;

public class TestTBOX : MonoBehaviour
{
    public TextBox textBox;
    public Button button;
    public string quack;

    private void Awake()
    {
      quack = "Quack! This is a test message for the TextBox.\n";
    }
    private void Start()
    {
        if (textBox == null)
            Debug.LogError("TestTBOX: TextBox reference is not set.");
        if (button == null)
            Debug.LogError("TestTBOX: Button reference is not set.");
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (textBox != null)
        {
            textBox.AddLine(quack);

        }
    }
}