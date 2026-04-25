using System;
using System.Collections.Generic;
using UnityEngine;

public enum Tutorials
{
    DuckClicked,
    GeneralInfo,
    Crumbies,
    Building,
    Journal,
    PlayPauseSpeed,
    DiningHall,
}

public class TutorialLines : MonoBehaviour
{
    public static TutorialLines reference;

    [Serializable] struct TutorialText
    {
        [SerializeField] Tutorials self;
        public string[] lines;
    }

    [SerializeField] TutorialText[] texts;
    bool[] textWasTriggered;

    void Awake()
    {
        reference = this;
    }

    void Start()
    {
        textWasTriggered = new bool[texts.Length];
    }

    public void TryActivate(Tutorials tutorial)
    {
        int num = (int)tutorial;
        if (num < 0 || num >= texts.Length) {return;}

        TutorialText text = texts[num];

        if (!textWasTriggered[num]) {
            TriggerMyPopup(text.lines);
            textWasTriggered[num] = true;
        }
    }

    void TriggerMyPopup(string[] lines)
    {
        if (PopupManager.Instance == null) return;

        List<PopupMessageData> generatedMessages = new();

        for (int i = 0; i < lines.Length; i++)
        {

            generatedMessages.Add(new PopupMessageData
            {
                textContent = lines[i],
                targetPosition = Vector2.zero
            });
        }

        PopupManager.Instance.StartPopupSequence(generatedMessages);
    }
}
