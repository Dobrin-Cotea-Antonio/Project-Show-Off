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
    [SerializeField] EnemyState[] enemyStates;
    [SerializeField] EnemyStateID initialState;

    Dictionary<EnemyStateID, EnemyState> enemyStateDictionary;
    public EnemyState activeState { get; private set; }

    #region Unity Events
    private void Awake() {
        hp = maxHp;

        OnDeath += AgentDeath;

        foreach (EnemyState state in enemyStates)
            enemyStateDictionary[state.stateID] = state;

        SwitchState(initialState);
    }

    private void Update() {
        activeState.Handle();
    }
    #endregion

    #region State Machine
    public void SwitchState(EnemyState pState) {
        OnExitStateWithID?.Invoke(activeState.stateID);
        OnExitStateOfType?.Invoke(activeState);

        activeState = pState;

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
}
