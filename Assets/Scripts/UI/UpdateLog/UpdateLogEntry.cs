using TMPro;
using UnityEngine;

public class UpdateLogEntry : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameField;
    [SerializeField] TextMeshProUGUI numberField;

    public void Reload(DuckUpgrade target)
    {
        nameField.text = target.UpgradeText;
        numberField.text = target.Level.ToString();
    }

    public void MakeBlank()
    {
        nameField.text = "";
        numberField.text = "";
    }
}
