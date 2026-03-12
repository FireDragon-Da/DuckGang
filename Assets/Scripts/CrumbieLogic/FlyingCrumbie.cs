using UnityEngine;
using System;

public class FlyingCrumbie : MonoBehaviour
{
    [SerializeField] float duration = 0.6f;
    [SerializeField] float arcHeight = 0.6f;
    [SerializeField] AnimationCurve moveCurve;

    Vector3 startPos;
    Vector3 targetPos;
    float timer;
    Action onArrive;

    public void Init(Vector3 start, Vector3 target, Action arriveCallback)
    {
        startPos = start;
        targetPos = target;
        onArrive = arriveCallback;
        timer = 0f;

        transform.position = startPos;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        Vector3 pos = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(t));

        float arc = Mathf.Sin(t * Mathf.PI) * arcHeight;
        pos.y += arc;

        transform.position = pos;

        if (t >= 1f)
        {
            onArrive?.Invoke();
            Destroy(gameObject);
        }
    }
}
