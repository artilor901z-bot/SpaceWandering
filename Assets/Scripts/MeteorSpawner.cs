using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab; // 陨石的 prefab
    public float spawnInterval = 2.0f; // 生成陨石的时间间隔
    public float meteorSpeed = 2.0f; // 陨石的移动速度
    public int maxMeteors = 2; // 最大陨石数量
    public int minMeteors = 1; // 最小陨石数量
    public float destroyTime = 10.0f; // 陨石飞出地图后销毁的时间
    public float minSize = 0.5f; // 陨石的最小大小
    public float maxSize = 1.5f; // 陨石的最大大小

    private float timer = 0.0f;
    private GameObject player; // 玩家对象

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // 假设玩家对象有 "Player" 标签
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0.0f;
            SpawnMeteors();
        }
    }

    void SpawnMeteors()
    {
        if (player == null) return;

        int meteorCount = Random.Range(minMeteors, maxMeteors + 1);

        for (int i = 0; i < meteorCount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect + 1,
                Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize),
                player.transform.position.z // 设置 z 轴位置为玩家的 z 轴位置
            );

            GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

            // 设置陨石的大小
            float size = Random.Range(minSize, maxSize);
            meteor.transform.localScale = new Vector3(size, size, size);

            meteor.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-meteorSpeed, 0);
            Destroy(meteor, destroyTime);
        }
    }
}

