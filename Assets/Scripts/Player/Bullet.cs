using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;
    public Vector3 initialSize = Vector3.one; // 初始大小
    public float damage = 10f; // Damage value

    void Start()
    {
        transform.localScale = initialSize; // 设置初始大小
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果碰撞对象的标签是 "Player" 或 "Bullet"，则忽略碰撞
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {
            return;
        }

        // 获取碰撞对象的 RangedEnemy 组件
        RangedEnemy enemy = collision.gameObject.GetComponent<RangedEnemy>();
        if (enemy != null)
        {
            // 对敌人造成伤害
            enemy.TakeDamage(damage);

            // 计算缩小比例，假设子弹的大小影响缩小比例
            float shrinkFactor = Mathf.Max(transform.localScale.magnitude * 0.1f, 0.1f);

            // 缩小敌人
            enemy.transform.localScale -= new Vector3(shrinkFactor, shrinkFactor, shrinkFactor);

            // 如果敌人的 scale 小于 1，则销毁敌人
            if (enemy.transform.localScale.magnitude < 1f)
            {
                Destroy(enemy.gameObject);
            }
        }

        // 销毁子弹
        Destroy(gameObject);
    }
}
