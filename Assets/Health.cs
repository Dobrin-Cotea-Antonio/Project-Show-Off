using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    
    AIAgent agent;
    
    UIHealthBar healthBar;
    
    void Start()
    {
        healthBar = GetComponentInChildren<UIHealthBar>();
        agent = GetComponent<AIAgent>();
        
        currentHealth = maxHealth;
        
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidbodies)
        {
            Hitbox hitbox = rigidBody.gameObject.AddComponent<Hitbox>();
            hitbox.health = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Die(Vector3.up);
        }
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            Die(direction);
        }
    }

    private void Die(Vector3 direction)
    {
        AIDeathState deathState = agent.stateMachine.GetState(AIStateId.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChageState(AIStateId.Death);
    }
}
