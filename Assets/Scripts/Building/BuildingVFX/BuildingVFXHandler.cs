using System.Collections;
using UnityEngine;

public class BuildingVFXHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetRenderer;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Color originalColor;

    private Coroutine scaleCoroutine;
    private Coroutine shakeCoroutine;
    private Coroutine colorCoroutine;

    private void Awake()
    {
        if (targetRenderer == null) targetRenderer = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        if (targetRenderer != null) originalColor = targetRenderer.color;
    }

    public void PlayEffect(BuildingVFXSO vfxConfig)
    {
        if (vfxConfig == null) return;

        if (vfxConfig.enableScaleBounce)
        {
            if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
            scaleCoroutine = StartCoroutine(ScaleRoutine(vfxConfig));
        }

        if (vfxConfig.enableShake)
        {
            if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(ShakeRoutine(vfxConfig));
        }

        if (vfxConfig.enableColorFlash && targetRenderer != null)
        {
            if (colorCoroutine != null) StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(ColorRoutine(vfxConfig));
        }
    }

    private IEnumerator ScaleRoutine(BuildingVFXSO config)
    {
        transform.localScale = originalScale;

        float timer = 0f;
        while (timer < config.scaleUpDuration)
        {
            timer += Time.deltaTime;
            float t = timer / config.scaleUpDuration;
            transform.localScale = Vector3.Lerp(originalScale, config.bounceScale, t);
            yield return null;
        }
        timer = 0f;
        while (timer < config.scaleDownDuration)
        {
            timer += Time.deltaTime;
            float t = timer / config.scaleDownDuration;
            transform.localScale = Vector3.Lerp(config.bounceScale, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    private IEnumerator ShakeRoutine(BuildingVFXSO config)
    {
        transform.localPosition = originalPosition;
        float timer = 0f;

        while (timer < config.shakeDuration)
        {
            timer += Time.deltaTime;
            Vector3 randomPoint = originalPosition + (Vector3)Random.insideUnitCircle * config.shakeIntensity;
            transform.localPosition = randomPoint;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    private IEnumerator ColorRoutine(BuildingVFXSO config)
    {
        targetRenderer.color = originalColor;
        float timer = 0f;
        float halfDuration = config.flashDuration / 2f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            targetRenderer.color = Color.Lerp(originalColor, config.flashColor, timer / halfDuration);
            yield return null;
        }

        timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            targetRenderer.color = Color.Lerp(config.flashColor, originalColor, timer / halfDuration);
            yield return null;
        }

        targetRenderer.color = originalColor;
    }
}