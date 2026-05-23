using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    // --- Singleton ---
    public static EnemyManager Instance;


    // --- Members ---
    [SerializeField] GameObject spawnArea;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] bool spawnEnemies = true;
    [SerializeField] float spawnInterval = 5f;
    [SerializeField] int maxEnemies = 10;
    [SerializeField] int numEnemiesSpawnAtOnce = 5;
    [SerializeField] int enemiesOnStart = 5;

    private List<EnemyController> enemies;


    // --- Methods ---
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        enemies = new List<EnemyController>();
        Debug.Assert(enemies != null, "EnemyManager: Failed to initialize enemy list.");

        Debug.Assert(spawnArea != null, "EnemyManager: Spawn area is not assigned.");
        Debug.Assert(enemyPrefabs != null && enemyPrefabs.Length > 0, "EnemyManager: Enemy prefabs are not assigned.");
    }

    private void Start()
    {
        if (spawnEnemies) {
            for (int i = 0; i < enemiesOnStart; i++) {
                GenerateEnemy();
            }
        }

        StartCoroutine(SpawnEnemiesCR());
    }

    private IEnumerator SpawnEnemiesCR()
    {
        // NOTE: Don't like infinite loops.
        while (true) {
            yield return new WaitForSeconds(spawnInterval);

            if (spawnEnemies) {
                if (enemies.Count < maxEnemies) {
                    int numEnemiesToSpawn = Mathf.Min(numEnemiesSpawnAtOnce, maxEnemies - enemies.Count);
                    for (int i = 0; i < numEnemiesToSpawn; i++) {
                        if (enemies.Count < maxEnemies) {
                            GenerateEnemy();
                        }
                        else {
                            break;
                        }
                    }
                }
            }

        }
    }

    public void GenerateEnemy()
    {
        if (spawnArea != null && enemyPrefabs != null && enemyPrefabs.Length > 0) {
            // Get the bounds of the spawn area
            Collider2D spawnAreaCollider = spawnArea.GetComponent<Collider2D>();
            if (spawnAreaCollider != null) {
                Bounds bounds = spawnAreaCollider.bounds;

                // Generate a random position within the bounds
                float randomX = Random.Range(bounds.min.x, bounds.max.x);
                float randomY = Random.Range(bounds.min.y, bounds.max.y);
                Vector2 spawnPosition = new Vector2(randomX, randomY);

                // Instantiate a random enemy prefab at the generated position
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyGO = Instantiate(
                    enemyPrefabs[randomIndex],
                    spawnPosition,
                    Quaternion.identity);

                // Add the enemy to the list of active enemies and subscribe to its death event
                if (enemyGO != null) {
                    EnemyController enemyController = enemyGO.GetComponent<EnemyController>();
                    if (enemyController != null) {
                        enemies.Add(enemyController);
                        enemyController.OnDeath += UnregisterEnemy;
                    }
                    else {
                        Debug.LogError("EnemyManager: Spawned enemy does not have an EnemyController component.");
                    }
                }
                else {
                    Debug.LogError("EnemyManager: Failed to instantiate enemy prefab.");
                }
            }
        }
    }

    public void UnregisterEnemy(ActorController enemy)
    {
        enemy.OnDeath -= UnregisterEnemy;

        if (enemy is EnemyController) {
            enemies.Remove(enemy as EnemyController);
        }
        else {
            Debug.LogError("EnemyManager: Attempted to unregister an actor that is not an EnemyController.");
        }
    }
}
