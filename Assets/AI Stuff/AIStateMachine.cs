using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    public AIState[] states;
    public AIAgent agent;
    public AIStateId currentStateId;

    public AIStateMachine(AIAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateId)).Length;
        states = new AIState[numStates];
    }
    
    public void RegisterState(AIState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }
    
    public AIState GetState(AIStateId id)
    {
        int index = (int)id;
        return states[index];
    }

    public void Upate()
    {
        GetState(currentStateId)?.Update(agent);
    }
    
    public void ChageState(AIStateId newState)
    {
        GetState(currentStateId)?.Exit(agent);
        currentStateId = newState;
        GetState(currentStateId)?.Enter(agent);
    }
}
