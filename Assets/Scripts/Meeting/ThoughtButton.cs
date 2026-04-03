using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtButton : MonoBehaviour
{
    public Image buttonImage;
    [HideInInspector] public DuckThought thought;
    public Toggle toggle;
    bool actuallyOn;
    public bool ActuallyOn => actuallyOn;
    [SerializeField] TextMeshProUGUI textField;
    [SerializeField] TextMeshProUGUI descField;

    public void SetupButton()
    {
        if (thought == null)
        {
            textField.text = "Blank";
            descField.text = "";
        }
        else
        {
            textField.text = thought.ThoughtText;
            descField.text = thought.DescriptionText;
        }
    }

    public void Select()
    {
        actuallyOn = true;
        buttonImage.color = Color.gray;
    }

    public void UnSelect()
    {
        actuallyOn = false;
        buttonImage.color = Color.white;
    }
}
