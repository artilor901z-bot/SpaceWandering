using UnityEngine;

public class AbsorbController : MonoBehaviour
{
    public float absorbRadius = 5f;
    public float absorbSpeed = 10f;
    public Transform hand;
    public Transform body;
    public GameManager gameManager; // 新增的 GameManager 引用

    private bool isAbsorbing = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAbsorbing = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAbsorbing = false;
        }

        if (isAbsorbing)
        {
            AbsorbCollectables();
        }
    }

    public void SetGameManager(GameManager manager) // 新增的设置 GameManager 的方法
    {
        gameManager = manager;
    }

    void AbsorbCollectables()
    {
        Collider2D[] collectables = Physics2D.OverlapCircleAll(body.position, absorbRadius);
        Vector3 handDirection = (hand.position - body.position).normalized;

        foreach (Collider2D collectable in collectables)
        {
            if (collectable.CompareTag("Collectable"))
            {
                Vector3 directionToCollectable = (collectable.transform.position - body.position).normalized;
                float dotProduct = Vector3.Dot(handDirection, directionToCollectable);

                // 优先吸引hand朝向的物体
                if (dotProduct > 0.5f) // 0.5f 是一个阈值，可以根据需要调整
                {
                    MoveCollectableTowardsPlayer(collectable);
                }
            }
        }
    }

    void MoveCollectableTowardsPlayer(Collider2D collectable)
    {
        Vector3 direction = (body.position - collectable.transform.position).normalized;
        collectable.transform.position = Vector3.MoveTowards(collectable.transform.position, body.position, absorbSpeed * Time.deltaTime);

        // 执行 collectable 的效果
        if (Vector3.Distance(collectable.transform.position, hand.position) < 1f)
        {
            ICollectable collectableItem = collectable.GetComponent<ICollectable>();
            if (collectableItem != null)
            {
                collectableItem.Collect(gameObject);
                gameManager.AddAmmo(5); // 吸收后增加5个子弹
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(body.position, absorbRadius);
    }
}

