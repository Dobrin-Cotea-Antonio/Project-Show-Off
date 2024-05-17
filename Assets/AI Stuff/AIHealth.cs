using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health
{
    AIAgent agent;
    protected override void OnStart()
    {
        agent = GetComponent<AIAgent>();
    }

    protected override void OnDeath(Vector3 direction)
    {
        AIDeathState deathState = agent.stateMachine.GetState(AIStateId.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChageState(AIStateId.Death);
    }
    protected override void OnDamage(Vector3 direction)
    {
        
    }
}
