using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour, IDamagable {

    public Action<float, float> OnDamageTaken;
    public Action<AIAgent> OnDeath;

    [Header("HP Data")]
    [SerializeField] float maxHp;
    public float hp { get; private set; }

    [Header("UI")]
    [SerializeField] UIHealthBar _healthBar;
    public UIHealthBar healthBar { get { return _healthBar; } }


    public AIStateMachine stateMachine;
    public AIStateId initialState;
    public AIAgentConfig config;
    public AIWeapons weapons;
    public Ragdoll ragdoll;
    public Transform playerTransform;

    public NavMeshAgent navMeshAgent { get; private set; }

    #region Unity Events
    private void Awake() {
        hp = maxHp;

        OnDamageTaken += AgentDamageTaken;
        OnDeath += AgentDeath;
    }

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        weapons = GetComponent<AIWeapons>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChastePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIAttackPlayerState());
        stateMachine.ChageState(initialState);

        ragdoll = GetComponent<Ragdoll>();
        //healthBar = GetComponentInChildren<UIHealthBar>();

        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update() {
        stateMachine.Upate();
    }
    #endregion

    #region Damage
    public void TakeDamage(float pDamage) {
        hp = Mathf.Max(hp - pDamage, 0);
        OnDamageTaken?.Invoke(hp, maxHp);
        if (hp == 0)
            OnDeath?.Invoke(this);
    }

    void AgentDeath(AIAgent pAgent) {
        AIDeathState deathState = stateMachine.GetState(AIStateId.Death) as AIDeathState;
        stateMachine.ChageState(AIStateId.Death);
        healthBar.gameObject.SetActive(false);
    }

    void AgentDamageTaken(float pCurrentHealth, float pMaxHealth) {
        //healthBar.SetHealthBarPercentage(pCurrentHealth / pMaxHealth);
        Debug.Log("Damaged Enemy");
    }
    #endregion


}
