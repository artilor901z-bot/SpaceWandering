using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int damage = 10;

    private Transform player;

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
        if (other.CompareTag("Player"))
        {
            // 对玩家造成伤害
            GameManager.Instance.TakeDamage(damage);
            // 销毁自己
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // 更新分数
        GameManager.Instance.AddScore(10);
    }
}