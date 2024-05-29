using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager instance { get; private set; }

    [Header("Wave Data")]
    [SerializeField] int waveCount;
    [SerializeField] int[] waveEnemyCount;
    [SerializeField] float[] delayBetweenSpawns;
    [SerializeField] float[] delayBetweenWaves;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] enemyPrefabs;

    [Header("Enemy Behavior")]
    [SerializeField] int maxChopEnemiesAllowed;

    List<EnemyAI> enemyList = new List<EnemyAI>();
    int currentEnemiesChopping = 0;

    [Header("Mast")]
    [SerializeField] Transform _mastTransform;
    public Transform mastTransform { get { return _mastTransform; } }

    [Header("Cover Points")]
    [SerializeField] List<Transform> coverPoints;
    List<Transform> freeCoverPoints = new List<Transform>();


    //remove
    [SerializeField] int currentWave = 0;

    #region Unity Events
    private void Awake() {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;

        freeCoverPoints = new List<Transform>(coverPoints);
        StartCoroutine(SpawnCoroutine());
    }

    private void Update() {
        ChopOrder();
    }
    #endregion

    #region Enemy Spawning
    void SpawnEnemy() {
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        int randomEnemyPrefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyObject = Instantiate(enemyPrefabs[randomEnemyPrefabIndex], spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);

        EnemyAI enemy = enemyObject.GetComponent<EnemyAI>();
        enemyList.Add(enemy);
        enemy.OnDeath += RemoveEnemy;
    }

    IEnumerator SpawnCoroutine() {
        currentWave = 0;

        while (currentWave < waveCount) {
            yield return new WaitForSeconds(delayBetweenWaves[currentWave]);

            Debug.Log("Started Wave");

            int spawnedEnemyCount = 0;

            while (spawnedEnemyCount < waveEnemyCount[currentWave]) {
                spawnedEnemyCount++;
                SpawnEnemy();
                yield return new WaitForSeconds(delayBetweenSpawns[currentWave]);
            }

            while (enemyList.Count != 0)
                yield return 0;

            currentWave++;
        }
    }
    #endregion

    #region Enemy Management
    private void RemoveEnemy(EnemyAI pEnemy) {
        enemyList.Remove(pEnemy);

        if (pEnemy.activeState.stateID == EnemyStateID.TargetMast || pEnemy.activeState.stateID == EnemyStateID.DamageMast)
            currentEnemiesChopping = Mathf.Max(currentEnemiesChopping - 1, 0);
    }

    private void ChopOrder() {
        if (enemyList.Count == 0)
            return;

        if (currentEnemiesChopping >= maxChopEnemiesAllowed)
            return;

        int randomIndex = Random.Range(0, enemyList.Count);
        //could have a more complex system for choosing who targets the mast like finding out which enemy is the closest
        //to the mast

        //also could implement multiple masts in the future
        enemyList[randomIndex].SwitchState(EnemyStateID.TargetMast);
        currentEnemiesChopping++;

        Debug.Log("Chop Order");
    }
    #endregion

    #region Cover Point Management
    public Transform FindClosestCoverPoint(FindCoverState pCoverState) {

        float minDistance = float.MaxValue;
        Transform closestPoint = null;

        NavMeshPath path = new NavMeshPath();

        foreach (Transform t in freeCoverPoints) {
            path.ClearCorners();
            float length = 0f;
            NavMesh.CalculatePath(pCoverState.transform.position, t.position, NavMesh.AllAreas, path);

            for (int i = 1; i < path.corners.Length; i++) {
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            //Debug.Log(length);

            if (length < minDistance) {
                minDistance = length;
                closestPoint = t;
            }
        }

        Debug.Log(closestPoint.position + " " + closestPoint.name);

        freeCoverPoints.Remove(closestPoint);

        return closestPoint;
    }

    //public Transform FindRandomCoverPoint(FindCoverState pCoverState) {
        //int randomIndex = Random.Range(0, freeCoverPoints.Count);



    //}

    public void MarkCoverPointAsEmpty(Transform pCoverPoint) {
        if (!coverPoints.Contains(pCoverPoint))
            return;

        if (freeCoverPoints.Contains(pCoverPoint))
            return;

        freeCoverPoints.Add(pCoverPoint);
    }
    #endregion
}
