using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshPathfinder : MonoBehaviour {
    public Action OnDeadzoneMoveStop;

    NavMeshAgent agent;

    Vector3 targetPosition;
    float agentSpeed;
    float distanceDeadzone;

    #region Unity Events
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update() {

        if ((targetPosition - transform.position).magnitude >= distanceDeadzone)
            return;

        if (agent.speed == 0)
            return;

        Stop();
        OnDeadzoneMoveStop?.Invoke();
    }
    #endregion

    #region Movement
    public void MoveTowardsTarget(Vector3 pTargetPosition, float pDistanceDeadzone = 0) {
        agent.SetDestination(pTargetPosition);

        targetPosition = pTargetPosition;
        distanceDeadzone = pDistanceDeadzone;
    }

    public void Stop() {
        agent.SetDestination(transform.position);
    }
    #endregion

    #region Helper Methods
    public float ReturnCurrentSpeed() {
        return agent.velocity.magnitude;
    }

    public void SetSpeed(float pSpeed) {
        agent.speed = pSpeed;
    }
    #endregion 
}
