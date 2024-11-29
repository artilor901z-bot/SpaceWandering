using UnityEngine;
using System.Collections;

public class CollectableItemGenerator : MonoBehaviour
{
    public GameObject[] collectablePrefabs; // 可收集物品预制体数组
    public Transform player; // 玩家位置
    public float spawnRadius = 10f; // 生成半径
    public float minSpawnDistance = 2f; // 最短生成距离
    public float spawnInterval = 1f; // 生成间隔时间
    public int maxCollectableCount = 10; // 可收集物品数量上限
    public float itemLifetime = 10f; // 物品的生命周期
    private float spawnTimer = 0f; // 生成计时器

    void Start()
    {
        StartCoroutine(SpawnCollectables());
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            if (CountCollectables() < maxCollectableCount)
            {
                SpawnCollectable();
            }
        }
    }

    IEnumerator SpawnCollectables()
    {
        while (true)
        {
            if (CountCollectables() < maxCollectableCount)
            {
                SpawnCollectable();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCollectable()
    {
        if (collectablePrefabs.Length == 0) return;

        // 随机选择一个可收集物品预制体
        GameObject collectablePrefab = collectablePrefabs[Random.Range(0, collectablePrefabs.Length)];

        // 随机生成位置在玩家周围的半径范围内，且距离玩家不小于最短生成距离
        Vector3 spawnPosition;
        do
        {
            spawnPosition = player.position + (Vector3)Random.insideUnitCircle * spawnRadius;
        } while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance);

        spawnPosition.z = 0; // 确保在2D平面上生成

        // 生成可收集物品
        GameObject collectable = Instantiate(collectablePrefab, spawnPosition, Quaternion.identity);

        // 在指定时间后销毁物品
        Destroy(collectable, itemLifetime);
    }

    int CountCollectables()
    {
        // 统计场景中可收集物品的数量
        return GameObject.FindGameObjectsWithTag("Collectable").Length;
    }
}