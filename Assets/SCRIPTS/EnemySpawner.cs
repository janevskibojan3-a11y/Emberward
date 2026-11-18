using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public DayNightCycle dayNightCycle;
    public GameObject shadowEnemyPrefab; // Drag your ShadowEnemy prefab here

    [Header("Spawn Settings")]
    public float spawnInterval = 4f;     // Seconds between enemy spawns at night
    public float minSpawnDistance = 9f;  // Keeps enemies from popping right on top of the campfire
    public float maxSpawnDistance = 13f; // Keeps enemies inside your tree borders

    private float spawnTimer = 0f;

    void Update()
    {
        // Only spawn enemies during the night!
        if (dayNightCycle == null || !dayNightCycle.isNight)
        {
            spawnTimer = 0f;
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnShadowEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnShadowEnemy()
    {
        if (shadowEnemyPrefab == null) return;

        // Pick a random direction and distance ring around the clearing center
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector3 spawnPos = transform.position + new Vector3(randomDir.x * distance, randomDir.y * distance, 0f);

        Instantiate(shadowEnemyPrefab, spawnPos, Quaternion.identity);
    }
}