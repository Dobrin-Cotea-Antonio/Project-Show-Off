using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager instance { get; private set; }

    public System.Action OnEnable;
    public System.Action OnDisable;
    public System.Action<int, int> OnWaveEnd;

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

    [Header("Mast")]
    [SerializeField] Transform _mastTransform;
    [SerializeField] float timeUntilNewTarget;

    public Transform mastTransform { get { return _mastTransform; } }
    List<EnemyAI> mastEnemyList = new List<EnemyAI>();

    [Header("Cover Points")]
    [SerializeField] List<Transform> coverPoints;
    List<Transform> freeCoverPoints = new List<Transform>();

    bool isEnabled = false;
    Coroutine enemySpawnCoroutine;
    Coroutine chopOrderCoroutine;

    //remove
    [SerializeField] int currentWave = 0;

    #region Unity Events
    private void Awake() {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;

        freeCoverPoints = new List<Transform>(coverPoints);

        OnEnable += ActivateSpawner;
        OnDisable += DisableSpawner;
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

    private IEnumerator SpawnCoroutine() {
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

            OnWaveEnd?.Invoke(currentWave, waveCount);

            currentWave++;
        }
    }
    #endregion

    #region Enemy Management
    private void RemoveEnemy(EnemyAI pEnemy) {
        enemyList.Remove(pEnemy);

        if (mastEnemyList.Contains(pEnemy)) {
            mastEnemyList.Remove(pEnemy);
            chopOrderCoroutine = StartCoroutine(ChopOrderCoroutine());
        }
    }

    private IEnumerator ChopOrderCoroutine() {
        yield return new WaitForSeconds(timeUntilNewTarget);

        while (true) {

            if (mastEnemyList.Count == maxChopEnemiesAllowed)
                yield break;

            if (enemyList.Count == 0) {
                yield return 0;
                continue;
            }

            if (!SceneManager.instance.isPlayerOnMast) {
                yield return 0;
                continue;
            }

            List<EnemyAI> availableEnemies = new List<EnemyAI>(enemyList);
            
            foreach (EnemyAI enemy in mastEnemyList)
                availableEnemies.Remove(enemy);

            foreach (EnemyAI enemy in availableEnemies)
                if (enemy.activeState.stateID == EnemyStateID.Death)
                    availableEnemies.Remove(enemy);

            if (availableEnemies.Count == 0) {
                yield return 0;
                continue;
            }

            int randomIndex = Random.Range(0, availableEnemies.Count);
            availableEnemies[randomIndex].SwitchState(EnemyStateID.TargetMast);
            mastEnemyList.Add(availableEnemies[randomIndex]);

            if (mastEnemyList.Count < maxChopEnemiesAllowed)
                chopOrderCoroutine = StartCoroutine(ChopOrderCoroutine());

            yield break;
        }
    }
    #endregion

    #region Cover Point Management
    public Transform FindClosestCoverPoint(FindCoverState pCoverState) {
        if (!isEnabled)
            return null;

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

            if (length < minDistance) {
                minDistance = length;
                closestPoint = t;
            }
        }

        Debug.Log(closestPoint.position + " " + closestPoint.name);

        freeCoverPoints.Remove(closestPoint);

        return closestPoint;
    }

    public Transform FindRandomCoverPoint(FindCoverState pCoverState) {
        if (!isEnabled)
            return null;

        ///////////////////////////
        int randomIndex = Random.Range(0, freeCoverPoints.Count);
        ///////////////////////////

        Debug.Log(enemyList.Count + " " + freeCoverPoints.Count + " " + randomIndex);

        Transform point = freeCoverPoints[randomIndex];
        freeCoverPoints.Remove(point);

        return point;
    }

    public void MarkCoverPointAsEmpty(Transform pCoverPoint) {
        if (!coverPoints.Contains(pCoverPoint))
            return;

        if (freeCoverPoints.Contains(pCoverPoint))
            return;

        Debug.Log("test");

        freeCoverPoints.Add(pCoverPoint);
    }
    #endregion

    #region State
    public void Enable(bool pState) {
        if (pState == isEnabled)
            return;

        isEnabled = pState;

        if (isEnabled)
            OnEnable?.Invoke();
        else
            OnDisable?.Invoke();
    }

    private void ActivateSpawner() {
        enemySpawnCoroutine = StartCoroutine(SpawnCoroutine());
        chopOrderCoroutine = StartCoroutine(ChopOrderCoroutine());
    }

    private void DisableSpawner() {
        StopCoroutine(enemySpawnCoroutine);
        StopCoroutine(chopOrderCoroutine);
    }
    #endregion
}
