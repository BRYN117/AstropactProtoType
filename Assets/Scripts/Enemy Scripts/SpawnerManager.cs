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
    public float safeDistanceFromPlayer = 12f;

    private float spawnY = 5f;

    // Endless mode parameters
    private bool endlessMode = false;
    private int endlessDifficulty = 1;
    private float endlessSpawnInterval = 10f;

    void Start()
    {
        // Update UI for the first wave
        WaveUIManager.instance.SetWave(currentWave);

        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (currentWave <= maxWaves)
        {
            // Update wave UI
            WaveUIManager.instance.SetWave(currentWave);

            Debug.Log("Starting Wave " + currentWave);

            // Spawn wave (one by one with delays)
            yield return StartCoroutine(SpawnWave(currentWave));

            // Wait until all enemies and asteroids are destroyed
            yield return new WaitUntil(() =>
                FindObjectsOfType<Asteroid>().Length == 0 &&
                FindObjectsOfType<EnemyHealth>().Length == 0
            );

            Debug.Log("Wave " + currentWave + " Cleared!");

            yield return new WaitForSeconds(timeBetweenWaves);

            currentWave++;
        }

        // All waves finished → start Endless Mode
        endlessMode = true;
        WaveUIManager.instance.SetEndless();

        Debug.Log("ALL WAVES COMPLETE — ENTERING ENDLESS MODE!");
        StartCoroutine(EndlessMode());
    }

    IEnumerator EndlessMode()
    {
        while (endlessMode)
        {
            Debug.Log("ENDLESS MODE SPAWN — Difficulty: " + endlessDifficulty);

            // Spawn enemies slowly
            for (int i = 0; i < endlessDifficulty; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(enemySpawnDelay);
            }

            // Spawn asteroids slowly
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

        // Spawn asteroids one-by-one
        for (int i = 0; i < asteroidCount; i++)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(asteroidSpawnDelay);
        }

        // Spawn enemies one-by-one
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
        GameObject prefab =
            pick == 0 ? smallAsteroid :
            pick == 1 ? mediumAsteroid :
                        largeAsteroid;

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

        // Snap to edges for cleaner spawning
        if (Random.value < 0.5f)
            x = (Random.value < 0.5f) ? -border : border;
        else
            z = (Random.value < 0.5f) ? -border : border;

        return new Vector3(x, spawnY, z);
    }

    // Returns a spawn point that avoids the player and overlapping objects
    Vector3 GetSafeSpawnPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return GetRandomSpawnPosition();

        Vector3 playerPos = player.transform.position;

        // Try up to 40 safe spawn attempts
        for (int i = 0; i < 40; i++)
        {
            Vector3 pos = GetRandomSpawnPosition();

            // Check distance from player
            if (Vector3.Distance(pos, playerPos) < safeDistanceFromPlayer)
                continue;

            // Check if something is already at this position
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

        // Fallback if no safe position is found
        Vector3 fallback = GetRandomSpawnPosition();
        Vector3 awayDir = (fallback - playerPos).normalized;

        return playerPos + awayDir * safeDistanceFromPlayer;
    }
}
