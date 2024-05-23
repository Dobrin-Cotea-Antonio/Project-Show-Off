using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [Header("Wave Data")]
    [SerializeField] int waveCount;
    [SerializeField] int[] waveEnemyCount;
    [SerializeField] float[] delayBetweenSpawns;
    [SerializeField] float[] delayBetweenWaves;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] enemyPrefabs;

    #region Unity Events
    private void Awake() {
        StartCoroutine(SpawnCoroutine());
    }
    #endregion

    #region Enemy Spawning
    void SpawnEnemy() {
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        int randomEnemyPrefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randomEnemyPrefabIndex], spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);
    }

    IEnumerator SpawnCoroutine() {
        int currentWave = 0;

        while (currentWave < waveCount) {
            yield return new WaitForSeconds(delayBetweenWaves[currentWave]);

            Debug.Log("Started Wave");

            int spawnedEnemyCount = 0;

            while (spawnedEnemyCount < waveEnemyCount[currentWave]) {
                spawnedEnemyCount++;
                SpawnEnemy();
                yield return new WaitForSeconds(delayBetweenSpawns[currentWave]);
            }

            currentWave++;
        }
    }
    #endregion
}
