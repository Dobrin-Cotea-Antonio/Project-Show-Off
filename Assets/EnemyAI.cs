using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour, IDamagable {

    public Action<EnemyStateID> OnEnterStateWithID;
    public Action<EnemyStateID> OnExitStateWithID;

    public Action<EnemyState> OnEnterStateOfType;
    public Action<EnemyState> OnExitStateOfType;

    public Action<float, float> OnDamageTaken;
    public Action<EnemyAI> OnDeath;

    [Header("HP Data")]
    [SerializeField] float maxHp;
    public float hp { get; private set; }

    [Header("States")]
    [SerializeField] NavMeshPathfinder pathfinder;
    [SerializeField] EnemyState[] enemyStates;
    [SerializeField] EnemyStateID initialState;

    [Header("Data")]
    [SerializeField] float _speed;
    [SerializeField] float minTimeBetweenCoverChanges;
    [SerializeField] float maxTimeBetweenCoverChanges;

    public float speed { get { return _speed; } }

    Coroutine coverCoroutine;

    Dictionary<EnemyStateID, EnemyState> enemyStateDictionary = new Dictionary<EnemyStateID, EnemyState>();
    public EnemyState activeState { get; private set; }
    [SerializeField] EnemyStateID enemyStateID;

    #region Unity Events
    private void Awake() {
        hp = maxHp;

        OnDeath += AgentDeath;
        OnDamageTaken += FindNewCoverOnHit;
        OnExitStateOfType += StartCoverCoroutine;
        OnEnterStateOfType += CancelCoverCoroutine;

        foreach (EnemyState state in enemyStates)
            enemyStateDictionary[state.stateID] = state;

        SwitchState(initialState);

        pathfinder.SetSpeed(speed);
    }

    private void Update() {
        activeState.Handle();
        enemyStateID = activeState.stateID;
    }
    #endregion

    #region State Machine
    public void SwitchState(EnemyState pState) {

        if (pState == activeState)
            return;

        OnExitStateWithID?.Invoke(activeState.stateID);
        OnExitStateOfType?.Invoke(activeState);

        if (activeState != null)
            activeState.OnStateExit();

        activeState = pState;

        activeState.OnStateEnter();

        OnEnterStateWithID?.Invoke(activeState.stateID);
        OnEnterStateOfType?.Invoke(activeState);
    }

    public void SwitchState(EnemyStateID pStateID) {
        SwitchState(enemyStateDictionary[pStateID]);
    }
    #endregion

    #region Damage
    public void TakeDamage(float pDamage) {
        hp = Mathf.Max(hp - pDamage, 0);
        OnDamageTaken?.Invoke(hp, maxHp);

        if (hp == 0)
            OnDeath?.Invoke(this);
    }

    void AgentDeath(EnemyAI pAgent) {
        SwitchState(EnemyStateID.Death);
    }
    #endregion

    #region Cover 
    IEnumerator EnemyCoverChangeTimer() {

        if (minTimeBetweenCoverChanges <= 0 || maxTimeBetweenCoverChanges <= 0)
            yield break;

        if (minTimeBetweenCoverChanges > maxTimeBetweenCoverChanges) {
            float aug = maxTimeBetweenCoverChanges;
            maxTimeBetweenCoverChanges = minTimeBetweenCoverChanges;
            minTimeBetweenCoverChanges = aug;
        }

        float value = maxTimeBetweenCoverChanges - minTimeBetweenCoverChanges;

        float randomValue = UnityEngine.Random.Range(0, value);
        float time = randomValue + minTimeBetweenCoverChanges;

        yield return new WaitForSeconds(time);
        Debug.Log("Switching cover - coroutine finished");
        SwitchState(EnemyStateID.FindCover);

    }

    void CancelCoverCoroutine(EnemyState pState) {
        if (pState is FindCoverState) {
            Debug.Log("Switching cover - coroutine canceled");
            if (coverCoroutine != null)
                StopCoroutine(coverCoroutine);
        }
    }

    void StartCoverCoroutine(EnemyState pState) {
        if (pState is FindCoverState) {
            Debug.Log("Switching cover - coroutine started");
            coverCoroutine = StartCoroutine(EnemyCoverChangeTimer());
        }
    }

    void FindNewCoverOnHit(float pHp, float pMaxHp) {
        SwitchState(EnemyStateID.FindCover);
    }
    #endregion
}
