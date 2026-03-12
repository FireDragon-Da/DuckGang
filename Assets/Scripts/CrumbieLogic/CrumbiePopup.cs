using UnityEngine;
using TMPro;
using UnityEngine;
public class CrumbiePopup : MonoBehaviour
{

    [SerializeField] SpriteRenderer iconRenderer;
    [SerializeField] TextMeshPro textMesh;

    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float lifetime = 1f;
    [SerializeField] Vector3 moveDirection = new Vector3(0, 1, 0);

    float timer;

    Color iconColor;
    Color textColor;

    public void Setup(int amount)
    {
        textMesh.text = "+" + amount.ToString();

        iconColor = iconRenderer.color;
        textColor = textMesh.color;

        timer = lifetime;
    }

    public void Setdown(int amount)
    {
        textMesh.text = "-" + amount.ToString();

        iconColor = iconRenderer.color;
        textColor = textMesh.color;

        timer = lifetime;
    }

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        timer -= Time.deltaTime;
        float t = 1f - (timer / lifetime);

        float alpha = 1f - t;

        iconColor.a = alpha;
        textColor.a = alpha;

        iconRenderer.color = iconColor;
        textMesh.color = textColor;

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
