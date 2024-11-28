using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float pullRadius = 5f; // 黑洞的引力半径
    public float pullForce = 100f; // 黑洞的引力大小

    void Update()
    {
        // 获取玩家对象
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // 计算黑洞和玩家之间的距离
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // 如果玩家在引力半径范围内
            if (distance < pullRadius)
            {
                // 计算引力方向
                Vector3 direction = (transform.position - player.transform.position).normalized;

                // 施加引力
                Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.AddForce(direction * pullForce * Time.deltaTime);
                }
                else
                {
                    Debug.LogWarning("Player does not have a Rigidbody2D component.");
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 如果玩家与黑洞发生碰撞
        if (other.CompareTag("Player"))
        {
            // 玩家死亡逻辑
            Destroy(other.gameObject);
            // 你可以在这里添加更多的玩家死亡处理逻辑，例如游戏结束或重生
        }
    }

    void OnDrawGizmos()
    {
        // 设置 Gizmo 的颜色
        Gizmos.color = Color.red;

        // 绘制一个表示引力半径的球体
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}