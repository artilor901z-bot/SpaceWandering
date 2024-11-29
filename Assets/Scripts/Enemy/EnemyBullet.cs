using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 5;
    public float speed = 5f;

    private Vector3 direction;

    public void Initialize(Vector3 shootDirection)
    {
        direction = shootDirection.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Collectable"))
        {
            Destroy(gameObject);
        }
    }
}