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
    public float maxBulletDamage = 20f;
    public float rotationSpeed = 5f;
    public float handDistance = 1f;

    [Header("音效设置")]
    public AudioClip shootSound;
    [Range(0f, 1f)]
    public float shootSoundVolume = 1f;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private float currentBulletSize = 0.1f;
    private float currentForce;
    private bool isCharging = false;
    private bool isRightClickHeld = false;
    private Vector3 shootDirection;
    private bool tripleShotEnabled = false;
    private float bodyRotation = 0f;
    private HandAnimator handAnimator;

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

        handAnimator = hand.GetComponent<HandAnimator>();
        if (handAnimator == null)
        {
            Debug.LogError("HandAnimator component missing on hand object!");
        }

        // 初始化音频源
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = shootSoundVolume;
    }

    void Update()
    {
        HandleInput();
        UpdateHandAndBodyPosition();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            handAnimator?.PlayClickAnimation();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            shootDirection = (hand.position - body.position).normalized;

            if (GameManager.Instance.UseAmmo(isCharging ? 5 : 1))
            {
                Shoot();
            }

            // 播放射击音效
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound, shootSoundVolume);
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
    }

    void UpdateHandAndBodyPosition()
    {
        // 获取鼠标位置
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // 计算方向
        Vector3 direction = (mousePosition - transform.position).normalized;

        // 计算body应该旋转的目标角度
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 平滑旋转body
        bodyRotation = Mathf.LerpAngle(bodyRotation, targetAngle, Time.deltaTime * rotationSpeed);

        // 更新body的位置和旋转（添加180度使sprite朝向正确）
        body.position = transform.position;
        body.rotation = Quaternion.Euler(0, 0, bodyRotation + 180f);

        // 更新hand的位置 - 在body前方固定距离
        Vector3 handOffset = Quaternion.Euler(0, 0, bodyRotation) * Vector3.right * handDistance;
        hand.position = body.position + handOffset;
        hand.rotation = Quaternion.Euler(0, 0, bodyRotation); // 手的旋转保持原样，不需要加180度

        // 更新bulletSpawn的位置为手的位置
        bulletSpawn.position = hand.position;
    }

    void Shoot()
    {
        if (tripleShotEnabled)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector3 offset = Quaternion.Euler(0, 0, bodyRotation) * new Vector3(0, i * 0.5f, 0);
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
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.Euler(0, 0, bodyRotation));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.initialSize = Vector3.one * Mathf.Max(minBulletSize, currentBulletSize);
        bulletScript.damage = Mathf.Lerp(10f, maxBulletDamage, (currentBulletSize - minBulletSize) / (maxBulletSize - minBulletSize));

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb == null)
        {
            bulletRb = bullet.AddComponent<Rigidbody2D>();
        }
        bulletRb.gravityScale = 0;

        // 使用body的方向作为射击方向
        Vector3 bulletDirection = Quaternion.Euler(0, 0, bodyRotation) * Vector3.right;
        bulletRb.linearVelocity = bulletDirection * bulletSpeed;
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
