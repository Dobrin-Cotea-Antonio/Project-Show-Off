using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshPathfinder : MonoBehaviour {
    public Action OnDeadzoneMoveStop;

    NavMeshAgent agent;

    Animator animator;

    Vector3 targetPosition;
    float agentSpeed;
    float distanceDeadzone;

    #region Unity Events
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Update() {

        if (agent.isOnOffMeshLink)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);

/*            Vector3 direction = (endPos - agent.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);*/

            if(animator != null)
            {
                Debug.Log("Setting the animator");
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }

            if (agent.transform.position == endPos)
            {
                agent.CompleteOffMeshLink();
            }
        }


        if (agent.pathPending)
            return;

        if (!(agent.remainingDistance <= agent.stoppingDistance))
            return;

        if (agent.hasPath)
            return;

        if (agent.velocity.sqrMagnitude != 0)
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
        agent.stoppingDistance = distanceDeadzone;
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
