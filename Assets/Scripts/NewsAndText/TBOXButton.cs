using UnityEngine;
using UnityEngine.UI;

public class TBOXButton : MonoBehaviour
{
    public TextBox textBox;
    public Button button;
    public Button toggleButton; 
    public string quack;
    public int counter = 0;
    public QuacxiconSO gameQuaxicon;
    public bool isQuackorderly = true;

    private bool isTextBoxVisible = true;  

    private void Awake()
    {

    }

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);

        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleTextBox);
        }
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

    /// <summary>
    /// ÇÐ»»TextBoxµÄÏÔÊ¾/Òþ²Ø×´Ì¬
    /// </summary>
    public void ToggleTextBox()
    {
        if (textBox != null)
        {
            isTextBoxVisible = !isTextBoxVisible;
            textBox.gameObject.SetActive(isTextBoxVisible);
        }
    }

    /// <summary>
    /// ÏÔÊ¾TextBox
    /// </summary>
    public void ShowTextBox()
    {
        if (textBox != null)
        {
            isTextBoxVisible = true;
            textBox.gameObject.SetActive(true);
        }
    }

    public void HideTextBox()
    {
        if (textBox != null)
        {
            isTextBoxVisible = false;
            textBox.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTextBox();
        }
    }
}