using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;
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
    }

    public void Update(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
    }
}
