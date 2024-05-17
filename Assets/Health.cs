using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    UIHealthBar healthBar;

    void Start()
    {
        healthBar = GetComponentInChildren<UIHealthBar>();

        currentHealth = maxHealth;

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidbodies)
        {
            Hitbox hitbox = rigidBody.gameObject.AddComponent<Hitbox>();
            hitbox.health = this;
            if (hitbox.gameObject != gameObject)
                hitbox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
        }

        OnStart();
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        if (healthBar)
        {
            healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        }
        OnDamage(direction);
        if (currentHealth <= 0)
        {
            Die(direction);
        }
    }

    private void Die(Vector3 direction)
    {
        OnDeath(direction);
    }

    protected virtual void OnStart()
    {
    }

    protected virtual void OnDeath(Vector3 direction)
    {
    }

    protected virtual void OnDamage(Vector3 direction)
    {
    }
}