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

    public Transform target { get; private set; }

    float epsilon = 0.2f;

    #region State Handling
    public override void Handle() {
        pathFinder.MoveTowardsTarget(target.position, epsilon);
        animator.SetFloat("Speed", pathFinder.ReturnCurrentSpeed());
    }

    public override void OnStateEnter() {
        Transform transformToRestore = target;

        target = EnemyManager.instance.FindClosestCoverPoint(this);

        Debug.Log(target.name + " " + target.position);

        if (transformToRestore != null)
            EnemyManager.instance.MarkCoverPointAsEmpty(transformToRestore);

        pathFinder.MoveTowardsTarget(target.position, epsilon);

        pathFinder.OnDeadzoneMoveStop += SwitchState;
    }

    public override void OnStateExit() {
        pathFinder.OnDeadzoneMoveStop -= SwitchState;
    }
    #endregion

    #region Helper Methods
    void SwitchState() {
        owner.SwitchState(targetState);
    }
    #endregion
}