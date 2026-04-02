using UnityEngine;

public class TuningManager : MonoBehaviour
{

    public static TuningManager reference;

    [Header("Happiness")]
    public int loseOnWork = -4;
    public int playgroundGainInteract = 7;
    public int playgroundGainPassive = 3;
    public float passiveDrop = 0.07f;

    [Header("Debug")]
    public bool instaKillHappiness = false;


    void Awake()
    {
        if (reference == null) reference = this;
        else Destroy(this);
    }
}
