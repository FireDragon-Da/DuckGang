using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewspaperTestTrigger : MonoBehaviour
{
    [SerializeField] private NewspaperController newspaperController;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);

    }
    private void OnButtonClick()
    { 
            TriggerTestNewspaper();
    }


    public void TriggerTestNewspaper()
    {
        if (newspaperController == null) return;

        List<string> bornThisTurn = new List<string> { "John", "Cena", "Bob", "Alice" };

        List<DeathEvent> diedThisTurn = new List<DeathEvent>
        {
            new DeathEvent { duckName = "JohnQuackna", reason = DeathReason.OldAge },
            new DeathEvent { duckName = "Cena", reason = DeathReason.Suicide },
            new DeathEvent { duckName = "YCSM", reason = DeathReason.Starvation }
        };

        List<ArticleEvent> eventsThisTurn = new List<ArticleEvent>
        {
            new ArticleEvent
            {
                title = "The First Church",
                content = "The first church was erected on new, beautiful soil. Worship is permitted. Worship is encouraged. Worship is, actually, required. It remains to be seen how happy everyone is about this. Hail Cursor",
                priority = ArticlePriority.NewBuilding
            },
            new ArticleEvent
            {
                title = "Communism Adopted",
                content = "The ducks have chosen Communism. If we live, we live together. If we die, we die together. Thus is the nature of the communist. Except for the very special communist, who decides who will live and who will die based on their personal beliefs.",
                priority = ArticlePriority.SocialFormation
            }
        };

        newspaperController.UpdateNewspaper(
            80f,
            40f,
            "CHANGE IN QUACKLAND: The council decided to implement a new daily crumbie ration system.",
            bornThisTurn,
            diedThisTurn,
            eventsThisTurn
        );
    }
}