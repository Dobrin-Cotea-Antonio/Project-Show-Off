using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMastState : EnemyState {

    [Header("Navigation")]
    [SerializeField] NavMeshPathfinder pathFinder;
    [SerializeField] float moveDeadzone;
    [SerializeField] private float attackZone;

    [Header("State Transition")]
    [SerializeField] EnemyStateID targetState;

    [Header("Animation")]
    [SerializeField] Animator animator;

    Transform targetTransform;

    #region State Handling
    public override void OnStateEnter() {
        targetTransform = EnemyManager.instance.mastTransform;
        pathFinder.OnDeadzoneMoveStop += CheckAndStartAttackingMast;
        pathFinder.MoveTowardsTarget(targetTransform.position, moveDeadzone);
    }

    public override void OnStateExit() {
        pathFinder.OnDeadzoneMoveStop -= CheckAndStartAttackingMast;
        animator.SetFloat("Speed", 0);
    }

    public override void Handle() {
        pathFinder.MoveTowardsTarget(targetTransform.position, moveDeadzone);
        animator.SetFloat("Speed", pathFinder.ReturnCurrentSpeed());
    }
    #endregion

    #region Helper Methods
    void StartAttackingMast() {
        Debug.LogWarning("Starting to attack mast.");
        owner.SwitchState(targetState);
    }
    
    void CheckAndStartAttackingMast()
    {
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        Debug.Log($"Distance to mast: {distance}");

        if (distance <= attackZone && HasLineOfSightToTarget())
        {
            Debug.Log("Within attack range and has line of sight to mast.");
            StartAttackingMast();
        }
        else
        {
            Debug.Log("Not within attack range or does not have line of sight.");
            pathFinder.MoveTowardsTarget(targetTransform.position, moveDeadzone);
        }
    }
    
    bool HasLineOfSightToTarget()
    {
        RaycastHit hit;
        Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, directionToTarget, out hit, attackZone))
        {
            Debug.Log($"Raycast hit: {hit.collider.name}");
            return hit.transform == targetTransform;
        }

        return false;
    }
    #endregion
}