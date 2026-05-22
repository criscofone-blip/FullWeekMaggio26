using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Settings")]
    [SerializeField] private int maxEnemiesOnMap = 6;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private bool spawnOnStart = true;

    private readonly List<GameObject> spawnedEnemies = new List<GameObject>();
    private float nextSpawnTime;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnUntilMax();
        }

        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        RemoveDeadEnemies();

        if (Time.time >= nextSpawnTime)
        {
            TrySpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    // Rimuove dalla lista i nemici che sono stati distrutti.
    private void RemoveDeadEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }
    }

    // Prova a spawnare un nemico solo se non abbiamo raggiunto il limite massimo.
    private void TrySpawnEnemy()
    {
        if (enemyPrefab == null)
            return;

        if (spawnPoints == null || spawnPoints.Length == 0)
            return;

        if (spawnedEnemies.Count >= maxEnemiesOnMap)
            return;

        Transform selectedSpawnPoint = GetRandomSpawnPoint();

        GameObject enemy = Instantiate(
            enemyPrefab,
            selectedSpawnPoint.position,
            selectedSpawnPoint.rotation
        );

        spawnedEnemies.Add(enemy);
    }

    // Spawna nemici fino al limite massimo impostato nell’Inspector.
    private void SpawnUntilMax()
    {
        while (spawnedEnemies.Count < maxEnemiesOnMap)
        {
            TrySpawnEnemy();
        }
    }

    // Sceglie casualmente uno degli spawn point inseriti nell’Inspector.
    private Transform GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex];
    }
}