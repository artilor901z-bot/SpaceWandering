using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float shootInterval = 2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public int damage = 5;
    public float health = 100f; // Add health property
    public AudioClip shootSound; // 添加射击音效
    [Range(0f, 1f)]
    public float shootSoundVolume = 1f; // 添加音量控制

    private Transform player;
    private float shootTimer;
    private AudioSource audioSource; // 添加音频源

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shootTimer = shootInterval;
        audioSource = gameObject.AddComponent<AudioSource>(); // 初始化音频源
        audioSource.playOnAwake = false;
        audioSource.volume = shootSoundVolume;
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

            // 播放射击音效
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound, shootSoundVolume);
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
