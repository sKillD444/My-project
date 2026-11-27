using UnityEngine;
using System.Collections;

[System.Serializable]
public class WaveGroup
{
    public GameObject enemyPrefab;
    public int count = 5;
    public float spawnInterval = 1f;
}

[System.Serializable]
public class Wave
{
    public string waveName = "Wave 1";
    public WaveGroup[] groups;
}

public class EnemySpawn : MonoBehaviour
{
    public Wave[] waves;
    public float timeBetweenWaves = 5f;

    public Transform[] wayPoints;
    public Transform spawnPoints;

    private int currentWaveIndex = 0;
    private bool hasStarted = false;

    public void StartWaves()
    {
        if (hasStarted) return;
        hasStarted = true;
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];

            foreach (WaveGroup group in currentWave.groups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyPrefab);
                    yield return new WaitForSeconds(group.spawnInterval);
                }
            }

            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    public void SpawnEnemy(GameObject prefabToSpawn)
    {
        if (prefabToSpawn == null || wayPoints == null || wayPoints.Length == 0) return;

        Vector3 spawnPosition = spawnPoints != null ? spawnPoints.position : wayPoints[0].position;
        GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.waypoints = wayPoints;
        }
    }
}