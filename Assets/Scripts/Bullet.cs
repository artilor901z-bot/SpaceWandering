using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime = 3.0f; // 子弹的生命周期

    void Start()
    {
        // 在3秒后销毁子弹
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.TakeDamage();
            }
        }

        // 如果碰撞对象不是Player，销毁子弹
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // 销毁子弹
        }
    }
}