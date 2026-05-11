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

    Vector3 dragStart;
    bool isDragging;

    [SerializeField] float edgeScrollThreshold = 20f;

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

    bool CursorNotBusy => !BuildingPlacer.reference.Using && !DuckDragger.reference.InUse;

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

        if (Input.GetMouseButtonDown(0) && CursorNotBusy)
        {
            dragStart = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 dragCurrent = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position += dragStart - dragCurrent;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        if (DuckDragger.reference.InUse) {
            // Edge scrolling
            Vector2 mousePos = Mouse.current.position.ReadValue();
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            if (mousePos.x >= 0 && mousePos.x <= screenWidth &&
                mousePos.y >= 0 && mousePos.y <= screenHeight)
            {
                if (mousePos.x < edgeScrollThreshold) moveInput.x -= 1;
                else if (mousePos.x > screenWidth - edgeScrollThreshold) moveInput.x += 1;

                if (mousePos.y < edgeScrollThreshold) moveInput.y -= 1;
                else if (mousePos.y > screenHeight - edgeScrollThreshold) moveInput.y += 1;

                moveInput = Vector2.ClampMagnitude(moveInput, 1f);
            }
        }

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
