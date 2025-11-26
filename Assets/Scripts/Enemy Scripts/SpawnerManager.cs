using UnityEngine;
using System.Collections;

public class SpawnerManager : MonoBehaviour
{
    [Header("Asteroid Prefabs")]
    public GameObject smallAsteroid;
    public GameObject mediumAsteroid;
    public GameObject largeAsteroid;

    [Header("Enemy Prefab")]
    public GameObject enemyTurret;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public int maxWaves = 5;

    public float timeBetweenWaves = 4f;

    public int baseAsteroidCount = 5;
    public int baseEnemyCount = 1;

    private float spawnY = 5f;

    // Endless mode variables
    private bool endlessMode = false;
    private int endlessDifficulty = 1;
    private float endlessSpawnInterval = 10f;

    void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (currentWave <= maxWaves)
        {
            Debug.Log("Starting Wave " + currentWave);

            SpawnWave(currentWave);

            yield return new WaitUntil(() =>
                GameObject.FindObjectsOfType<Asteroid>().Length == 0 &&
                GameObject.FindObjectsOfType<EnemyHealth>().Length == 0
            );

            Debug.Log("Wave " + currentWave + " Cleared!");

            yield return new WaitForSeconds(timeBetweenWaves);

            currentWave++;
        }

        Debug.Log("ALL WAVES COMPLETE — ENTERING ENDLESS MODE!");
        endlessMode = true;
        StartCoroutine(EndlessMode());
    }

    IEnumerator EndlessMode()
    {
        while (endlessMode)
        {
            Debug.Log("ENDLESS MODE SPAWN — Difficulty: " + endlessDifficulty);

            // Spawn enemies
            for (int i = 0; i < endlessDifficulty; i++)
            {
                SpawnEnemy();
            }

            // Spawn asteroids
            for (int i = 0; i < endlessDifficulty; i++)
            {
                SpawnAsteroid();
            }

            // Increase difficulty per cycle
            endlessDifficulty++;

            // Wait for next endless cycle
            yield return new WaitForSeconds(endlessSpawnInterval);
        }
    }

    void SpawnWave(int waveIndex)
    {
        int asteroidCount = baseAsteroidCount + (waveIndex * 2);
        int enemyCount = baseEnemyCount + (waveIndex - 1);

        for (int i = 0; i < asteroidCount; i++)
            SpawnAsteroid();

        for (int i = 0; i < enemyCount; i++)
            SpawnEnemy();
    }

    void SpawnAsteroid()
    {
        Vector3 pos = GetRandomSpawnPosition();

        int pick = Random.Range(0, 3);
        GameObject prefab = smallAsteroid;

        if (pick == 1) prefab = mediumAsteroid;
        if (pick == 2) prefab = largeAsteroid;

        Instantiate(prefab, pos, Quaternion.identity);
    }

    void SpawnEnemy()
    {
        Vector3 pos = GetRandomSpawnPosition();
        Instantiate(enemyTurret, pos, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        float border = 25f;

        float x = Random.Range(-border, border);
        float z = Random.Range(-border, border);

        if (Random.value < 0.5f)
            x = (Random.value < 0.5f) ? -border : border;
        else
            z = (Random.value < 0.5f) ? -border : border;

        return new Vector3(x, spawnY, z);
    }
}
