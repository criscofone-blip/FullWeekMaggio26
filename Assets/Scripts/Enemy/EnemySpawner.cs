using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn")]
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnRate = 4f;

    private float nextSpawnTime;
    private int currentEnemies;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        TrySpawnEnemy();
    }

    // Controlla se può spawnare un nuovo nemico.
    private void TrySpawnEnemy()
    {
        if (Time.time < nextSpawnTime)
            return;

        if (currentEnemies >= maxEnemies)
            return;

        Transform spawnPoint = GetSpawnPointOutsideCamera();

        if (spawnPoint == null)
            return;

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        currentEnemies++;
        nextSpawnTime = Time.time + spawnRate;
    }

    // Cerca uno spawn point che non sia visibile dalla camera.
    private Transform GetSpawnPointOutsideCamera()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (!IsVisibleFromCamera(spawnPoint.position))
            {
                return spawnPoint;
            }
        }

        return null;
    }

    // Controlla se un punto è dentro la visuale della camera.
    private bool IsVisibleFromCamera(Vector3 worldPosition)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(worldPosition);

        bool isInFrontOfCamera = viewportPoint.z > 0f;

        bool isInsideViewport =
            viewportPoint.x > 0f &&
            viewportPoint.x < 1f &&
            viewportPoint.y > 0f &&
            viewportPoint.y < 1f;

        return isInFrontOfCamera && isInsideViewport;
    }
}