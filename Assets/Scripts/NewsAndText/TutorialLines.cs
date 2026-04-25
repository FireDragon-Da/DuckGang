using System;
using System.Collections.Generic;
using UnityEngine;

public enum Tutorials
{
    DuckClicked,
    TopLeftInfo,
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
        public bool triggered;
        public string[] lines;
    }

    [SerializeField] TutorialText duckClicked;

    void Awake()
    {
        reference = this;
    }

    public void TryActivate(Tutorials tutorial)
    {
        switch (tutorial)
        {
            case Tutorials.DuckClicked:
                if (!duckClicked.triggered) {
                    TriggerMyPopup(duckClicked.lines);
                    duckClicked.triggered = true;
                }
                break;
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
