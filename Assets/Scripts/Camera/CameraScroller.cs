using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField] float xRange;
    [SerializeField] float yRange;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        targetSize = cam.orthographicSize;
    }

    void Update()
    {
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        transform.Translate(speed * moveInput * Time.deltaTime);

        Vector3 newPos = transform.position + speed * Time.deltaTime * (Vector3)moveInput;
        newPos.x = Mathf.Clamp(newPos.x, -xRange, xRange);
        newPos.y = Mathf.Clamp(newPos.y, -yRange, yRange);
        transform.position = newPos;

        float zoomInput = playerInput.actions["Zoom"].ReadValue<float>();

        if (zoomInput != 0)
        {
            targetSize -= zoomInput * zoomSpeed * Time.deltaTime;
            targetSize = Mathf.Clamp(targetSize, minSize, maxSize);
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * 10);

    }
}
