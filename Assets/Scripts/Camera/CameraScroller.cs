using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class CameraScroller : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] Camera cam;
    [SerializeField] float speed;

    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minSize = 5f;
    [SerializeField] float maxSize = 10f;
    float targetSize;

    [SerializeField] Tilemap tilemap;

    Vector3 worldMin;
    Vector3 worldMax;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        targetSize = cam.orthographicSize;

        Bounds worldBounds = tilemap.localBounds;
        worldMin = tilemap.transform.TransformPoint(worldBounds.min);
        worldMax = tilemap.transform.TransformPoint(worldBounds.max);

        //Bandaid for weird issue with map size
        worldMin.x++;
        worldMax.x--;
        worldMax.y--;
    }

    void Update()
    {
        float zoomInput = playerInput.actions["Zoom"].ReadValue<float>();


        if (zoomInput != 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            targetSize -= zoomInput * zoomSpeed * Time.unscaledDeltaTime;
            targetSize = Mathf.Clamp(targetSize, minSize, maxSize);

            float oldSize = cam.orthographicSize;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.unscaledDeltaTime * 10);

            float sizeRatio = cam.orthographicSize / oldSize;
            transform.position = mouseWorldBefore + (transform.position - mouseWorldBefore) * sizeRatio;
        }

        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 newPos = transform.position + speed * Time.unscaledDeltaTime * (Vector3)moveInput;

        float verticalExtent = cam.orthographicSize;
        float horizontalExtent = cam.orthographicSize * cam.aspect;

        float minX = worldMin.x + horizontalExtent;
        float maxX = worldMax.x - horizontalExtent;

        float minY = worldMin.y + verticalExtent;
        float maxY = worldMax.y - verticalExtent;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        newPos.z = 0;

        transform.position = newPos;

    }
}
