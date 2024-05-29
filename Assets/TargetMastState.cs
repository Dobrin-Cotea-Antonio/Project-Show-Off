using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMastState : EnemyState {

    [Header("Navigation")]
    [SerializeField] NavMeshPathfinder pathFinder;
    [SerializeField] float moveDeadzone;

    [Header("State Transition")]
    [SerializeField] EnemyStateID targetState;

    [Header("Animation")]
    [SerializeField] Animator animator;

    Transform targetTransform;

    #region Unity Events
    private void Awake() {
        targetTransform = EnemyManager.instance.mastTransform;
    }
    #endregion

    #region State Handling
    public override void OnStateEnter() {
        pathFinder.OnDeadzoneMoveStop += StartAttackingMast;
    }

    public override void OnStateExit() {
        pathFinder.OnDeadzoneMoveStop -= StartAttackingMast;
        animator.SetFloat("Speed", 0);
    }

    public override void Handle() {
        pathFinder.MoveTowardsTarget(targetTransform.position, moveDeadzone);
        animator.SetFloat("Speed", pathFinder.ReturnCurrentSpeed());
    }
    #endregion

    #region Helper Methods
    void StartAttackingMast() {
        owner.SwitchState(targetState);
    }
    #endregion
}