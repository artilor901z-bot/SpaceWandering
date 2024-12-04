using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int damage = 10;
    public float health = 100f;
    public AudioClip deathSound; // 添加死亡音效
    [Range(0f, 1f)]
    public float deathSoundVolume = 1f; // 添加音量控制

    private Transform player;
    private bool hasDealtDamage = false; // 添加标志位
    private AudioSource audioSource; // 添加音频源

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = gameObject.AddComponent<AudioSource>(); // 初始化音频源
        audioSource.playOnAwake = false;
        audioSource.volume = deathSoundVolume;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDealtDamage) return; // 如果已经造成伤害，则直接返回

        if (other.CompareTag("Player"))
        {
            Debug.Log($"MeleeEnemy dealing {damage} damage to player.");
            GameManager.Instance.TakeDamage(damage);
            hasDealtDamage = true; // 设置标志位为 true
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                Destroy(other.gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // 播放死亡音效
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound, deathSoundVolume);
        }

        GameManager.Instance.AddScore(10);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        GameManager.Instance.AddScore(10);
    }
}
