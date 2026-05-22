using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Patrol Points")]
    [SerializeField] private Transform[] patrolPoints;

    [Header("Settings")]
    [SerializeField] private int maxEnemiesOnMap = 5;
    [SerializeField] private float spawnInterval = 4f;

    private readonly List<GameObject> spawnedEnemies = new();

    private float nextSpawnTime;

    private void Update()
    {
        RemoveDeadEnemies();

        if (Time.time >= nextSpawnTime)
        {
            TrySpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    // Rimuove nemici distrutti dalla lista.
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

    // Prova a spawnare un nemico.
    private void TrySpawnEnemy()
    {
        if (spawnedEnemies.Count >= maxEnemiesOnMap)
            return;

        if (spawnPoints.Length == 0)
            return;

        Transform randomSpawn =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemyObject = Instantiate(
            enemyPrefab,
            randomSpawn.position,
            randomSpawn.rotation
        );

        EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();

        if (enemyAI != null)
        {
            enemyAI.SetPlayer(player);
            enemyAI.SetPatrolPoints(patrolPoints);
        }

        spawnedEnemies.Add(enemyObject);
    }
}