using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        targetSize = cam.orthographicSize;
    }

    void Update()
    {
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 newPos = transform.position + speed * Time.deltaTime * (Vector3)moveInput;

        float zoomInput = playerInput.actions["Zoom"].ReadValue<float>();

        if (zoomInput != 0)
        {
            targetSize -= zoomInput * zoomSpeed * Time.deltaTime;
            targetSize = Mathf.Clamp(targetSize, minSize, maxSize);
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * 10);

        Bounds worldBounds = tilemap.localBounds;
        Vector3 worldMin = tilemap.transform.TransformPoint(worldBounds.min);
        Vector3 worldMax = tilemap.transform.TransformPoint(worldBounds.max);

        float verticalExtent = cam.orthographicSize;
        float horizontalExtent = cam.orthographicSize * cam.aspect;

        float minX = worldMin.x + horizontalExtent;
        float maxX = worldMax.x - horizontalExtent;

        float minY = worldMin.y + verticalExtent;
        float maxY = worldMax.y - verticalExtent;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = newPos;

    }
}
