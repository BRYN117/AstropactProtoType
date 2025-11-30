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

    [Header("Spawn Timing")]
    public float asteroidSpawnDelay = 0.25f;
    public float enemySpawnDelay = 0.5f;

    [Header("Spawn Safety")]
    public float safeDistanceFromPlayer = 12f; // recommended value

    private float spawnY = 5f;

    // Endless mode
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

            yield return StartCoroutine(SpawnWave(currentWave));

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

            for (int i = 0; i < endlessDifficulty; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(enemySpawnDelay);
            }

            for (int i = 0; i < endlessDifficulty; i++)
            {
                SpawnAsteroid();
                yield return new WaitForSeconds(asteroidSpawnDelay);
            }

            endlessDifficulty++;
            yield return new WaitForSeconds(endlessSpawnInterval);
        }
    }

    IEnumerator SpawnWave(int waveIndex)
    {
        int asteroidCount = baseAsteroidCount + (waveIndex * 2);
        int enemyCount = baseEnemyCount + (waveIndex - 1);

        for (int i = 0; i < asteroidCount; i++)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(asteroidSpawnDelay);
        }

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    void SpawnAsteroid()
    {
        Vector3 pos = GetSafeSpawnPosition();

        int pick = Random.Range(0, 3);
        GameObject prefab = smallAsteroid;

        if (pick == 1) prefab = mediumAsteroid;
        if (pick == 2) prefab = largeAsteroid;

        Instantiate(prefab, pos, Quaternion.identity);
    }

    void SpawnEnemy()
    {
        Vector3 pos = GetSafeSpawnPosition();
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

    // 100% SAFE SPAWN POSITION
    Vector3 GetSafeSpawnPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return GetRandomSpawnPosition();

        Vector3 playerPos = player.transform.position;

        for (int i = 0; i < 40; i++)
        {
            Vector3 pos = GetRandomSpawnPosition();

            // Distance check
            if (Vector3.Distance(pos, playerPos) < safeDistanceFromPlayer)
                continue;

            // Physics overlap check to avoid objects on top of each other
            Collider[] hits = Physics.OverlapSphere(pos, 3f);
            bool blocked = false;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Asteroid") || hit.CompareTag("Enemy"))
                {
                    blocked = true;
                    break;
                }
            }

            if (blocked)
                continue;

            return pos;
        }

        // Fallback if no good position found
        Vector3 fallback = GetRandomSpawnPosition();
        Vector3 awayDir = (fallback - playerPos).normalized;

        return playerPos + awayDir * safeDistanceFromPlayer;
    }
}
