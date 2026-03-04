using UnityEngine;

public class TriggerExample : MonoBehaviour
{
    [SerializeField] private QuacxiconSO gameQuaxicon; 
    [SerializeField] private TextBox textBox;

    Rigidbody rb;
    Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grass"))
        {
            TriggerQuackLog();

        }
    }
    public void TriggerQuackLog()
    {
        string log = gameQuaxicon.GetRandomLogFromCategory("Grass");
        textBox.AddLine(log);
    }
}
