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
        //pathFinder.OnDeadzoneMoveStop += CheckAndStartAttackingMast;
        CheckAndStartAttackingMast();
        pathFinder.MoveTowardsTarget(targetTransform.position, moveDeadzone);
    }

    public override void OnStateExit() {
        pathFinder.OnDeadzoneMoveStop -= CheckAndStartAttackingMast;
        animator.SetFloat("Speed", 0);
    }

    public override void Handle() {
        pathFinder.MoveTowardsTarget(targetTransform.position, moveDeadzone);
        animator.SetFloat("Speed", pathFinder.ReturnCurrentSpeed());
        
        CheckAndStartAttackingMast();
    }
    #endregion

    #region Helper Methods
    void StartAttackingMast() {
        owner.SwitchState(targetState);
    }
    
    void CheckAndStartAttackingMast()
    {
        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if (distance <= attackZone)
        {
            StartAttackingMast();
        }
        else
        {
            pathFinder.MoveTowardsTarget(targetTransform.position, moveDeadzone);
            animator.SetFloat("Speed", pathFinder.ReturnCurrentSpeed());
        }
    }
    #endregion
}