using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveForce = 10f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform hand;
    public Transform body;
    public float maxBulletSize = 2f;
    public float minBulletSize = 0.5f;
    public float maxForce = 20f;
    public float zoomSpeed = 0.5f;
    public float bulletSizeGrowthRate = 1f;
    public float bulletSpeed = 15f;
    public float maxBulletDamage = 20f; // Maximum bullet damage

    private Rigidbody2D rb;
    private float currentBulletSize = 0.1f;
    private float currentForce;
    private bool isCharging = false;
    private bool isRightClickHeld = false;
    private Vector3 shootDirection; // 用于存储射击方向
    private bool tripleShotEnabled = false; // 是否启用三连发

    // 定义一个事件来通知摄像头重置缩放
    public static event Action OnShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = 1; // 使用 drag 而不是 linearDamping
        currentForce = moveForce;

        // 确保 AbsorbController 的 hand 和 body 被正确设置
        AbsorbController absorbController = GetComponent<AbsorbController>();
        if (absorbController != null)
        {
            absorbController.hand = hand;
            absorbController.body = body;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;

            // 计算射击方向，确保每次射击方向固定
            shootDirection = (hand.position - body.position).normalized;

            if (GameManager.Instance.UseAmmo(isCharging ? 5 : 1)) // 检查弹药是否足够
            {
                Shoot();
            }
            currentBulletSize = 0.1f;
            currentForce = moveForce;
        }

        if (isCharging)
        {
            currentBulletSize = Mathf.Min(maxBulletSize, currentBulletSize + Time.deltaTime * bulletSizeGrowthRate);
            currentForce = Mathf.Min(maxForce, currentForce + Time.deltaTime * moveForce);
        }

        if (Input.GetMouseButtonDown(1))
        {
            isRightClickHeld = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightClickHeld = false;
        }

        UpdateHandPosition();
    }

    void Shoot()
    {
        if (tripleShotEnabled)
        {
            // 三连发射击
            for (int i = -1; i <= 1; i++)
            {
                Vector3 offset = new Vector3(i * 0.5f, 0, 0); // 调整子弹的偏移量
                CreateBullet(bulletSpawn.position + offset);
            }
        }
        else
        {
            // 单发射击
            CreateBullet(bulletSpawn.position);
        }

        // Apply force to player in the opposite direction
        rb.AddForce(-shootDirection * currentForce, ForceMode2D.Impulse);

        // 触发事件通知摄像头重置缩放
        OnShoot?.Invoke();
    }

    void CreateBullet(Vector3 position)
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.initialSize = Vector3.one * Mathf.Max(minBulletSize, currentBulletSize);
        bulletScript.damage = Mathf.Lerp(10f, maxBulletDamage, (currentBulletSize - minBulletSize) / (maxBulletSize - minBulletSize)); // Set bullet damage based on size

        // 确保子弹具有 Rigidbody2D 组件
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb == null)
        {
            bulletRb = bullet.AddComponent<Rigidbody2D>();
        }
        bulletRb.gravityScale = 0; // 确保子弹不受重力影响
        bulletRb.linearVelocity = shootDirection * bulletSpeed; // 设置子弹速度为固定值
    }

    void UpdateHandPosition()
    {
        // 更新 hand 的位置，但不改变 shootDirection
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - body.position).normalized;
        hand.position = body.position + direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isRightClickHeld && other.CompareTag("Collectable"))
        {
            ICollectable collectable = other.GetComponent<ICollectable>();
            if (collectable != null)
            {
                collectable.Collect(gameObject);
            }
        }
    }

    public void EnableTripleShot()
    {
        tripleShotEnabled = true;
        // 设置一个计时器来在10秒后禁用三连发
        Invoke("DisableTripleShot", 5f); // 10秒后禁用三连发
    }

    void DisableTripleShot()
    {
        tripleShotEnabled = false;
    }
}
