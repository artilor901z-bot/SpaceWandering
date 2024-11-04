using UnityEngine;

public class AbsorptionController : MonoBehaviour
{
    public float absorptionRange = 10.0f; // 吸收范围
    public float absorptionSpeed = 5.0f; // 吸收速度
    public LayerMask asteroidLayer; // 陨石层
    public GameObject backPack; // 背包对象
    public float maxBackPackScale = 2.0f; // 背包大小上限
    public GameObject targetPositionObject; // 目标位置对象

    private GameObject handObject;
    private GameObject targetAsteroid;

    void Start()
    {
        handObject = GetComponent<PlayerController>().handObject;
        // targetPositionObject 可以在外部设置，因此不再需要在 Start 方法中初始化
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 右键按下
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetMouseDirection(), absorptionRange, asteroidLayer);
            if (hit.collider != null)
            {
                targetAsteroid = hit.collider.gameObject;
            }
        }

        if (targetAsteroid != null)
        {
            MoveAsteroidTowardsHand();
        }
    }

    Vector2 GetMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (mousePosition - transform.position).normalized;
    }

    void MoveAsteroidTowardsHand()
    {
        if (targetAsteroid == null) return;

        Vector3 direction = (targetPositionObject.transform.position - targetAsteroid.transform.position).normalized;
        targetAsteroid.transform.position += direction * absorptionSpeed * Time.deltaTime;

        // 输出调试信息
        Debug.Log($"Asteroid Position: {targetAsteroid.transform.position}");
        Debug.Log($"Target Position: {targetPositionObject.transform.position}");
        Debug.Log($"Distance: {Vector3.Distance(targetAsteroid.transform.position, targetPositionObject.transform.position)}");

        if (Vector3.Distance(targetAsteroid.transform.position, targetPositionObject.transform.position) < 0.1f)
        {
            IncreaseBackPackSize(targetAsteroid.transform.localScale.x);
            Destroy(targetAsteroid);
            targetAsteroid = null;
        }
    }

    void IncreaseBackPackSize(float asteroidScale)
    {
        float scaleIncrease = asteroidScale < 0.5f ? 0.1f : 0.3f;
        Vector3 newScale = backPack.transform.localScale + new Vector3(scaleIncrease, scaleIncrease, 0);

        // 确保背包的大小不会超过上限
        newScale.x = Mathf.Min(newScale.x, maxBackPackScale);
        newScale.y = Mathf.Min(newScale.y, maxBackPackScale);

        backPack.transform.localScale = newScale;
    }
}