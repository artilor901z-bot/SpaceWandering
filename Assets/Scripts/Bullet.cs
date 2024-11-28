using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;
    public Vector3 initialSize = Vector3.one; // 初始大小
    public float damage = 10f; // 子弹伤害
    public float minShrinkFactor = 0.1f; // 最小缩小比例

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

        // 获取碰撞对象的 Transform 组件
        Transform targetTransform = collision.transform;

        // 计算缩小比例，假设子弹的大小影响缩小比例
        float shrinkFactor = Mathf.Max(transform.localScale.magnitude * 0.1f, minShrinkFactor);

        // 缩小碰撞对象
        targetTransform.localScale -= new Vector3(shrinkFactor, shrinkFactor, shrinkFactor);

        // 如果碰撞对象的 scale 小于 1，则销毁碰撞对象
        if (targetTransform.localScale.magnitude < 1f)
        {
            Destroy(targetTransform.gameObject);
        }

        // 销毁子弹
        Destroy(gameObject);
    }
}