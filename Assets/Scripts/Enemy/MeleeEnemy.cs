using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int damage = 10;
    public float health = 100f;

    private Transform player;
    private bool hasDealtDamage = false; // 添加标志位

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        GameManager.Instance.AddScore(10);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        GameManager.Instance.AddScore(10);
    }
}
