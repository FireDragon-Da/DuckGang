using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Image buttonImage;
    [HideInInspector] public DuckUpgrade upgrade;

    [SerializeField] TextMeshProUGUI textField;
    [SerializeField] TextMeshProUGUI descField;

    public void SetupButton()
    {
        textField.text = upgrade.UpgradeText;
        descField.text = upgrade.DescriptionText;
    }
}
