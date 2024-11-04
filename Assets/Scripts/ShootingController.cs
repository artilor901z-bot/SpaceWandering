using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab; // 子弹预制体
    public Transform firePoint; // 发射点
    public float bulletSpeed = 10f; // 子弹速度
    public float recoilForce = 2f; // 后坐力强度
    public GameObject backPack; // 背包对象
    public float minPlayerScale = 0.1f; // 最小允许射击的玩家大小
    public float scaleReduction = 0.1f; // 每次射击减少的大小
    public float recoilCooldown = 0.5f; // 后坐力冷却时间

    private bool canApplyRecoil = true; // 标志是否可以应用后坐力

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检查左键按下
        {
            if (CanShoot())
            {
                Shoot();
            }
        }
    }

    bool CanShoot()
    {
        // 检查对象的大小
        return backPack.transform.localScale.x >= minPlayerScale && backPack.transform.localScale.y >= minPlayerScale;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;
        rb.linearVelocity = shootDirection * bulletSpeed;

        // 禁用子弹的重力
        rb.gravityScale = 0;

        // 添加反作用力
        if (canApplyRecoil)
        {
            Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
            playerRb.AddForce(-shootDirection * recoilForce, ForceMode2D.Impulse);
            canApplyRecoil = false;
            Invoke("ResetRecoil", recoilCooldown); // 在冷却时间后重置后坐力标志
        }

        // 减少背包的大小
        Vector3 newScale = backPack.transform.localScale - new Vector3(scaleReduction, scaleReduction, 0);
        backPack.transform.localScale = newScale;
    }

    void ResetRecoil()
    {
        canApplyRecoil = true;
    }
}
