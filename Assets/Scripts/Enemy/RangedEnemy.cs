using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float shootInterval = 2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public int damage = 5;
    public float health = 100f; // Add health property

    private Transform player;
    private float shootTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shootTimer = shootInterval;
    }

    void Update()
    {
        // 随机移动
        transform.position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * moveSpeed * Time.deltaTime;

        // 瞄准玩家并射击
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            if (enemyBullet != null)
            {
                enemyBullet.Initialize(direction);
                enemyBullet.damage = damage;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // 更新分数
        GameManager.Instance.AddScore(10);
    }
}