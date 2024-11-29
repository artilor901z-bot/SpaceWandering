using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 敌人预制体数组
    public Transform player; // 玩家位置
    public float spawnRadius = 10f; // 生成半径
    public float minSpawnDistance = 5f; // 最短生成距离
    public float spawnInterval = 5f; // 生成间隔时间
    public int initialEnemyCount = 3; // 初始生成的敌人数量
    public int maxEnemyCount = 5; // 初始敌人数量上限
    private int currentEnemyCount; // 当前生成的敌人数量
    private float spawnTimer = 0f; // 生成计时器
    private float limitIncreaseTimer = 0f; // 上限增加计时器

    void Start()
    {
        currentEnemyCount = initialEnemyCount;
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        limitIncreaseTimer += Time.deltaTime;

        if (limitIncreaseTimer >= 20f)
        {
            maxEnemyCount += 2; // 每过20秒，敌人数量上限+2
            limitIncreaseTimer = 0f; // 重置计时器
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (CountEnemies() < maxEnemyCount)
            {
                for (int i = 0; i < currentEnemyCount; i++)
                {
                    if (CountEnemies() >= maxEnemyCount)
                    {
                        break; // 如果敌人数量达到上限，停止生成
                    }
                    SpawnEnemy();
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        // 随机选择一个敌人预制体
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // 随机生成位置在玩家周围的半径范围内，且距离玩家不小于最短生成距离
        Vector3 spawnPosition;
        do
        {
            spawnPosition = player.position + (Vector3)Random.insideUnitCircle * spawnRadius;
        } while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance);

        spawnPosition.z = 0; // 确保在2D平面上生成

        // 生成敌人
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    int CountEnemies()
    {
        // 统计场景中敌人的数量
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
