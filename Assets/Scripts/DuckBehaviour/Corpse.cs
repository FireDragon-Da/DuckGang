using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Corpse : MonoBehaviour
{
    [SerializeField] float stayTime;
    [SerializeField] float disappearTime;
    float curTime;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > stayTime+disappearTime)
        {
            Destroy(gameObject);
            return;
        }

        if (curTime > stayTime)
        {
            Color tempColor = spriteRenderer.color;
            tempColor.a = (disappearTime-(curTime-stayTime))/disappearTime;
            spriteRenderer.color = tempColor;
        }
    }
}
