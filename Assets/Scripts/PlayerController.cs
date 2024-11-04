using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5.0f;
    public float airControlForce = 2.0f;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject boundaryMinObject; // 边界的最小值对象
    public GameObject boundaryMaxObject; // 边界的最大值对象
    public GameObject flipObject; // 用于反转的子对象
    public GameObject handObject; // 玩家手部对象
    public float handDistance = 1.0f; // 手部对象与玩家之间的距离
    public Transform firePoint; // 发射点
    public GameObject targetPositionObject; // 目标位置对象

    private Rigidbody2D rb;
    private Vector2 boundaryMin;
    private Vector2 boundaryMax;
    private bool cameraStoppedFollowing = false; // 摄像机是否停止跟随
    [SerializeField]
    private EnergyManager energyManager;
    private ShootingController shootingController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 设置正常的重力
        rb.gravityScale = 0.001f;

        // 获取边界的最小值和最大值
        if (boundaryMinObject != null && boundaryMaxObject != null)
        {
            boundaryMin = boundaryMinObject.transform.position;
            boundaryMax = boundaryMaxObject.transform.position;
        }

        energyManager = GetComponent<EnergyManager>();
        shootingController = GetComponent<ShootingController>();
        gameObject.AddComponent<AbsorptionController>(); // 添加吸收控制器
    }

    void Update()
    {
        // 检查玩家是否按下空格键
        if (Input.GetKeyDown(KeyCode.Space) && energyManager.ConsumeEnergy(energyManager.energyConsumptionPerJump))
        {
            // 直接设置玩家的速度
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 在空中时应用小的推力来模拟移动
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * airControlForce * Time.deltaTime, ForceMode2D.Force);
        }

        // 检查玩家是否超出边界
        CheckBoundary();

        // 检查鼠标位置并改变玩家朝向
        CheckMousePosition();

        // 更新手部位置
        UpdateHandPosition();
    }

    void CheckBoundary()
    {
        Vector3 playerPosition = transform.position;

        // 检查玩家是否超出边界
        if (playerPosition.x < boundaryMin.x || playerPosition.x > boundaryMax.x ||
            playerPosition.y < boundaryMin.y || playerPosition.y > boundaryMax.y)
        {
            if (!cameraStoppedFollowing)
            {
                // 停止摄像机跟随
                virtualCamera.Follow = null;
                cameraStoppedFollowing = true;
            }
        }
        else
        {
            if (cameraStoppedFollowing)
            {
                // 恢复摄像机跟随
                virtualCamera.Follow = transform;
                cameraStoppedFollowing = false;
            }
        }
    }

    void CheckMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 localScale = flipObject.transform.localScale;

        if (mousePosition.x < transform.position.x)
        {
            // 鼠标在玩家左边，玩家朝向左
            localScale.x = -Mathf.Abs(localScale.x);
        }
        else
        {
            // 鼠标在玩家右边，玩家朝向右
            localScale.x = Mathf.Abs(localScale.x);
        }

        flipObject.transform.localScale = localScale;
    }

    void UpdateHandPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // 确保 z 轴为 0
        Vector3 direction = (mousePosition - flipObject.transform.position).normalized;
        handObject.transform.position = flipObject.transform.position + direction * handDistance;
        targetPositionObject.transform.position = handObject.transform.position; // 更新目标位置对象的位置
    }
}
