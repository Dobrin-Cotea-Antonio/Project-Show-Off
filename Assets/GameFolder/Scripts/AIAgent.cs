using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    public AIWeapons weapons;
    public Ragdoll ragdoll;
    public UIHealthBar healthBar;
    public Transform playerTransform;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        weapons = GetComponent<AIWeapons>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChastePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIAttackPlayerState());
        stateMachine.ChageState(initialState);
        
        ragdoll = GetComponent<Ragdoll>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        stateMachine.Upate();
    }
}
