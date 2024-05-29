using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChastePlayerState : AIState
{
    float timer = 0.0f;

    public AIStateId GetId()
    {
        return AIStateId.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
    }

    public void Update(AIAgent agent)
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            float sqDistance = (agent.playerTransform.position - agent.navMeshAgent.destination).sqrMagnitude;
            if (sqDistance > agent.config.maxDistance * agent.config.maxDistance)
            {
                agent.navMeshAgent.destination = agent.playerTransform.position;
            }

            timer = agent.config.maxTime;
        }
    }

    public void Exit(AIAgent agent)
    {
    }
}