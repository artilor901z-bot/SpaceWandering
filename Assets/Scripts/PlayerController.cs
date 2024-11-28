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

    private Rigidbody2D rb;
    private float currentBulletSize = 0.1f;
    private float currentForce;
    private bool isCharging = false;
    private Vector3 shootDirection; // 用于存储射击方向

    // 定义一个事件来通知摄像头重置缩放
    public static event Action OnShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = 1;
        currentForce = moveForce;
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

            Shoot();
            currentBulletSize = 0.1f;
            currentForce = moveForce;
        }

        if (isCharging)
        {
            currentBulletSize = Mathf.Min(maxBulletSize, currentBulletSize + Time.deltaTime * bulletSizeGrowthRate);
            currentForce = Mathf.Min(maxForce, currentForce + Time.deltaTime * moveForce);
        }

        UpdateHandPosition();
    }

    void Shoot()
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().initialSize = Vector3.one * Mathf.Max(minBulletSize, currentBulletSize);

        // 确保子弹具有 Rigidbody2D 组件
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb == null)
        {
            bulletRb = bullet.AddComponent<Rigidbody2D>();
        }
        bulletRb.gravityScale = 0; // 确保子弹不受重力影响
        bulletRb.linearVelocity = shootDirection * bulletSpeed; // 设置子弹速度为固定值

        // Apply force to player in the opposite direction
        rb.AddForce(-shootDirection * currentForce, ForceMode2D.Impulse);

        // 触发事件通知摄像头重置缩放
        OnShoot?.Invoke();
    }

    void UpdateHandPosition()
    {
        // 更新 hand 的位置，但不改变 shootDirection
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - body.position).normalized;
        hand.position = body.position + direction;
    }
}