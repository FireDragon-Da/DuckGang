using System.Collections;
using System.Collections;
using UnityEngine;

/// <summary>
/// Always-active singleton that periodically captures a screenshot centered on a random duck.
/// Stores the two most recent captured sprites so NewspaperController can display them
/// whenever the newspaper is opened, regardless of whether it was open during capture.
/// </summary>
public class DuckPhotoCaptureManager : MonoBehaviour
{
    public static DuckPhotoCaptureManager reference;

    [Header("Photo Capture Settings")]
    [SerializeField] private float photoCaptureInterval = 60f;
    [SerializeField] private int photoWidth = 512;
    [SerializeField] private int photoHeight = 512;
    [SerializeField] private float photoCaptureZoom = 3f;

    // Latest captured sprites, persisted between newspaper open/close cycles
    public Sprite LatestPhoto1 { get; private set; }
    public Sprite LatestPhoto2 { get; private set; }

    private float photoTimer;
    private Camera mainCamera;
    private int currentPhotoIndex = 0;

    void Awake()
    {
        if (reference == null)
        {
            reference = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        mainCamera = Camera.main;

        // Trigger first capture immediately at game start, then repeat on interval
        photoTimer = 0f;
    }

    void Update()
    {
        photoTimer -= Time.deltaTime;
        if (photoTimer <= 0f)
        {
            photoTimer = photoCaptureInterval;
            CaptureDuckPhoto();
        }
    }

    private void CaptureDuckPhoto()
    {
        GameObject randomDuck = GetRandomDuck();
        if (randomDuck == null)
        {
            return;
        }

        StartCoroutine(CaptureScreenshotCoroutine(randomDuck));
    }

    private GameObject GetRandomDuck()
    {
        GameObject[] allDucks = GameObject.FindGameObjectsWithTag("Duck");
        if (allDucks.Length == 0)
        {
            return null;
        }

        return allDucks[Random.Range(0, allDucks.Length)];
    }

    private IEnumerator CaptureScreenshotCoroutine(GameObject targetDuck)
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 originalPosition = mainCamera.transform.position;
        float originalSize = mainCamera.orthographicSize;

        Vector3 duckPosition = targetDuck.transform.position;
        mainCamera.transform.position = new Vector3(duckPosition.x, duckPosition.y, mainCamera.transform.position.z);
        mainCamera.orthographicSize = photoCaptureZoom;

        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(photoWidth, photoHeight, 24);
        RenderTexture currentRT = RenderTexture.active;
        mainCamera.targetTexture = renderTexture;

        mainCamera.Render();

        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
        screenshot.Apply();

        mainCamera.targetTexture = null;
        RenderTexture.active = currentRT;
        Destroy(renderTexture);

        mainCamera.transform.position = originalPosition;
        mainCamera.orthographicSize = originalSize;

        StoreCapture(screenshot);
    }

    private void StoreCapture(Texture2D photoTexture)
    {
        Sprite photoSprite = Sprite.Create(
            photoTexture,
            new Rect(0, 0, photoTexture.width, photoTexture.height),
            new Vector2(0.5f, 0.5f)
        );

        if (currentPhotoIndex == 0)
        {
            LatestPhoto1 = photoSprite;
            currentPhotoIndex = 1;
        }
        else
        {
            LatestPhoto2 = photoSprite;
            currentPhotoIndex = 0;
        }
    }
}
