using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset from the player
    public float zoomSpeed = 0.1f; // 缩放速度
    public float minZoom = 5f; // 最小缩放
    public float maxZoom = 10f; // 最大缩放
    public float initialZoom = 7.5f; // 初始缩放

    private Camera cam;
    private bool isZoomingIn = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = initialZoom; // 设置初始缩放

        // 订阅事件
        PlayerController.OnShoot += ResetZoom;
    }

    void OnDestroy()
    {
        // 取消订阅事件
        PlayerController.OnShoot -= ResetZoom;
    }

    void Update()
    {
        if (player != null)
        {
            // Set the camera's position to the player's position plus the offset
            transform.position = player.position + offset;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isZoomingIn = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isZoomingIn = false;
        }

        if (isZoomingIn)
        {
            cam.orthographicSize = Mathf.Max(minZoom, cam.orthographicSize - zoomSpeed * Time.deltaTime);
        }
        else
        {
            cam.orthographicSize = Mathf.Min(maxZoom, cam.orthographicSize + zoomSpeed * Time.deltaTime);
        }
    }

    void ResetZoom()
    {
        cam.orthographicSize = initialZoom; // 重置缩放到初始值
    }
}