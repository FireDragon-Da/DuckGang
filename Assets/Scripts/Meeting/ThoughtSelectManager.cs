using UnityEngine;
using UnityEngine.UI;

public class ThoughtSelectManager : MonoBehaviour
{
    [HideInInspector] public int maxSelections = 3;
    int currentSelections = 0;

    public ThoughtButton[] options;

    bool disabling;

    void Start()
    {
        foreach (ThoughtButton button in options)
        {
            button.toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(button, isOn));
        }
    }

    void OnToggleChanged(ThoughtButton changedToggle, bool isOn)
    {
        if (disabling) {return;}

        if (isOn)
        {
            if (currentSelections >= maxSelections)
            {
                //Remove selection
                changedToggle.toggle.isOn = false;
            }
            else
            {
                changedToggle.Select();
                currentSelections++;
            }
        }
        else
        {
            if (changedToggle.ActuallyOn)
            {
                changedToggle.UnSelect();
                currentSelections--;
            }
        }
    }

    public void ResetSelections()
    {
        disabling = true;
        foreach (ThoughtButton button in options)
        {
            button.UnSelect();
            button.toggle.isOn = false;
        }
        currentSelections = 0;
        disabling = false;
    }

}
