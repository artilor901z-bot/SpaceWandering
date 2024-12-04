using UnityEngine;
using System;

public class PlayerController2 : MonoBehaviour
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
    public float maxBulletDamage = 20f;
    public float bodyRotationSpeed = 10f;
    public float handRotationSpeed = 15f; // 手部旋转速度
    public float handDistance = 1f; // 手部距离身体的距离

    private Rigidbody2D rb;
    private float currentBulletSize = 0.1f;
    private float currentForce;
    private bool isCharging = false;
    private bool isRightClickHeld = false;
    private Vector3 shootDirection;
    private bool tripleShotEnabled = false;

    public static event Action OnShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = 1;
        currentForce = moveForce;

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
            shootDirection = (hand.position - body.position).normalized;

            if (GameManager.Instance.UseAmmo(isCharging ? 5 : 1))
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

        UpdateRotationAndPosition();
    }

    void UpdateRotationAndPosition()
    {
        // 获取鼠标世界坐标
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // 计算方向向量
        Vector3 bodyToMouse = (mousePosition - body.position).normalized;

        // 更新身体旋转
        float bodyAngle = Mathf.Atan2(bodyToMouse.y, bodyToMouse.x) * Mathf.Rad2Deg;
        Quaternion targetBodyRotation = Quaternion.Euler(0, 0, bodyAngle);
        body.rotation = Quaternion.Lerp(body.rotation, targetBodyRotation, Time.deltaTime * bodyRotationSpeed);

        // 更新手部位置和旋转
        Vector3 handDirection = (mousePosition - body.position).normalized;
        Vector3 targetHandPosition = body.position + handDirection * handDistance;

        // 平滑移动手部位置
        hand.position = Vector3.Lerp(hand.position, targetHandPosition, Time.deltaTime * handRotationSpeed);

        // 更新手部朝向
        float handAngle = Mathf.Atan2(handDirection.y, handDirection.x) * Mathf.Rad2Deg;
        Quaternion targetHandRotation = Quaternion.Euler(0, 0, handAngle);
        hand.rotation = Quaternion.Lerp(hand.rotation, targetHandRotation, Time.deltaTime * handRotationSpeed);

        // 更新射击方向
        shootDirection = handDirection;
    }

    void Shoot()
    {
        if (tripleShotEnabled)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector3 offset = new Vector3(i * 0.5f, 0, 0);
                CreateBullet(bulletSpawn.position + offset);
            }
        }
        else
        {
            CreateBullet(bulletSpawn.position);
        }

        rb.AddForce(-shootDirection * currentForce, ForceMode2D.Impulse);
        OnShoot?.Invoke();
    }

    void CreateBullet(Vector3 position)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.initialSize = Vector3.one * Mathf.Max(minBulletSize, currentBulletSize);
        bulletScript.damage = Mathf.Lerp(10f, maxBulletDamage, (currentBulletSize - minBulletSize) / (maxBulletSize - minBulletSize));

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb == null)
        {
            bulletRb = bullet.AddComponent<Rigidbody2D>();
        }
        bulletRb.gravityScale = 0;
        bulletRb.linearVelocity = shootDirection * bulletSpeed;
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
        Invoke("DisableTripleShot", 5f);
    }

    void DisableTripleShot()
    {
        tripleShotEnabled = false;
    }
}