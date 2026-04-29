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

    [SerializeField] Sprite regSprite;
    [SerializeField] Sprite selectedSprite;

    public void SetupButton()
    {
        textField.text = upgrade.UpgradeText;
        descField.text = upgrade.DescriptionText;
    }

    public void Select()
    {
        buttonImage.sprite = selectedSprite;
    }

    public void UnSelect()
    {
        buttonImage.sprite = regSprite;
    }
}
