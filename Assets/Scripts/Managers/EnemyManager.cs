using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//using System;

public class EnemyManager : MonoBehaviour
{
    // --- Singleton ---

    public static EnemyManager Instance;


    // --- Members ---

    [Header("--- EnemyManager Settings ---")]
    [SerializeField] bool spawnEnemies = true;
    [SerializeField] GameObject spawnArea;
    [SerializeField] WeightedList<GameObject> enemyPrefabs;
    [SerializeField] float spawnInterval = 5f;
    [SerializeField] int maxEnemies = 10;
    [SerializeField] int numEnemiesSpawnAtOnce = 5;
    [SerializeField] int enemiesOnStart = 5;

    private List<EnemyController> enemiesInScene;


    // --- Methods ---

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        enemiesInScene = new List<EnemyController>();
        Debug.Assert(enemiesInScene != null, "EnemyManager: Failed to initialize enemy list.");

        // NOTE: Because we assign the enemies and weights in the inspector,
        // we need to call UpdateWeights() to ensure the totalCoins weight is calculated
        // correctly.
        if (enemyPrefabs != null) {
            enemyPrefabs.UpdateWeights();
        }
        else {
            Debug.LogError("EnemyManager: Enemy prefabs list is not assigned.");
        }

        Debug.Assert(spawnArea != null, "EnemyManager: Spawn area is not assigned.");

        // For ease of use with the editor.
        //
        // Here we check how many enemies are already in the scene and add them to our list
        // of enemies, so we can subscribe to their OnDeath event and manage them properly.
        Object[] existingEnemies = Object.FindObjectsByType<EnemyController>();
        foreach (Object enemyObject in existingEnemies) {

            EnemyController enemy = enemyObject as EnemyController;
            if (enemy != null) {
                enemiesInScene.Add(enemy);
                SubscribeEvents(enemy);
            }
        }
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
                if (enemiesInScene.Count < maxEnemies) {
                    int numEnemiesToSpawn = Mathf.Min(numEnemiesSpawnAtOnce, maxEnemies - enemiesInScene.Count);
                    for (int i = 0; i < numEnemiesToSpawn; i++) {
                        if (enemiesInScene.Count < maxEnemies) {
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
        if (spawnArea != null && enemyPrefabs != null && enemyPrefabs.list.Count > 0) {
            // Get the bounds of the spawn area
            Collider2D spawnAreaCollider = spawnArea.GetComponent<Collider2D>();
            if (spawnAreaCollider != null) {
                Bounds bounds = spawnAreaCollider.bounds;

                // Generate a random position within the bounds
                float randomX = Random.Range(bounds.min.x, bounds.max.x);
                float randomY = Random.Range(bounds.min.y, bounds.max.y);
                Vector2 spawnPosition = new Vector2(randomX, randomY);

                // Instantiate a random enemy prefab at the generated position
                //int randomIndex = Random.Range(0, enemyPrefabs.Length);

                GameObject randomEnemyPrefab = enemyPrefabs.GetRandomItem();
                //GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)].Item1;

                GameObject enemyGO = Instantiate(
                    randomEnemyPrefab,
                    spawnPosition,
                    Quaternion.identity);

                if (enemyGO != null) {
                    EnemyController enemy = enemyGO.GetComponent<EnemyController>();
                    if (enemy != null) {

                        // Add the newly generated enemy to the list of enemies in the scene.
                        enemiesInScene.Add(enemy);

                        SubscribeEvents(enemy);
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


    // --- Event Handlers ---

    private void SubscribeEvents(EnemyController enemy)
    {
        // Register a callback to trigger OnDeath logic.
        enemy.OnDeath += () => EnemyDieHandler(enemy);

        // Register a callback to remove the enemy from the list when it is destroyed.
        enemy.OnDestroy += () => UnregisterEnemy(enemy);
    }

    private void EnemyDieHandler(EnemyController deadEnemy)
    {
        // Here we handle what happens when an enemy dies. Drop loot and stuff.
        DropManager.Instance.HandleDrop(deadEnemy);
    }


    public void UnregisterEnemy(EnemyController enemy)
    {
        enemiesInScene.Remove(enemy);
    }
}
