using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;

    public event Action OnDeath;
    
    public AIStateId GetId()
    {
        return AIStateId.Death;
    }

    public void Enter(AIAgent agent)
    {
        agent.ragdoll.ActivateRagdoll();
        direction.y = 1.0f;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        agent.healthBar.gameObject.SetActive(false);
        
        OnDeath?.Invoke();
        
        agent.StartCoroutine(DestroyAfterTime(agent));
    }

    public void Update(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
    }
    
    IEnumerator DestroyAfterTime(AIAgent agent)
    {
        yield return new WaitForSeconds(agent.config.destroyTime);
        GameObject.Destroy(agent.gameObject);
    }
}
