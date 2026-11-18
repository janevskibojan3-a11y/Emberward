using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject woodPrefab; // Drag your wood prefab here
    public float spawnInterval = 10f; // How many seconds between each spawn
    public float spawnRadius = 8f;   // How far from the center wood can spawn

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnWood();
            spawnTimer = 0f;
        }
    }

    void SpawnWood()
    {
        if (woodPrefab == null) return;

        // Pick a random position inside a circle around the spawner/campfire
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        // Instantiate the wood prefab
        Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
    }
}