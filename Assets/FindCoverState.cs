using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCoverState : EnemyState {

    [Header("Navigation")]
    [SerializeField] NavMeshPathfinder pathFinder;

    [Header("State Transition")]
    [SerializeField] EnemyStateID targetState;

    [Header("Animation")]
    [SerializeField] Animator animator;

    [SerializeField] bool useRandomCover = false;

    public Transform target { get; private set; }

    float epsilon = 0.1f;

    #region Unity Events
    private void Awake() {
        owner.OnDeath += ClearTarget;
    }
    #endregion

    #region State Handling
    public override void Handle() {
        pathFinder.MoveTowardsTarget(target.position, epsilon);
        animator.SetFloat("Speed", pathFinder.ReturnCurrentSpeed());
    }

    public override void OnStateEnter() {
        Transform transformToRestore = target;

        if (useRandomCover)
            target = EnemyManager.instance.FindRandomCoverPoint(this);
        else
            target = EnemyManager.instance.FindClosestCoverPoint(this);

        if (target == null) {
            SwitchState();
            return;
        }

        if (transformToRestore != null)
            EnemyManager.instance.MarkCoverPointAsEmpty(transformToRestore);

        pathFinder.OnDeadzoneMoveStop += SwitchState;
        //switched the 2 around
        pathFinder.MoveTowardsTarget(target.position, epsilon);
    }

    public override void OnStateExit() {
        pathFinder.OnDeadzoneMoveStop -= SwitchState;
    }
    #endregion

    #region Helper Methods
    void SwitchState() {
        owner.SwitchState(targetState);
    }

    void ClearTarget(EnemyAI pEnemy) {
        if (target != null)
            EnemyManager.instance.MarkCoverPointAsEmpty(target);
    }
    #endregion
}