using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState {

    [Header("Navigation")]
    [SerializeField] NavMeshPathfinder pathFinder;
    [SerializeField] float moveDeadzone;

    [Header("State Transition")]
    [SerializeField] EnemyStateID targetState;

    [Header("Animation")]
    [SerializeField] Animator animator;

    Transform playerTransform;

    #region Unity Events
    private void Start() {
        playerTransform = SceneManager.instance.playerGameObject.transform;
    }
    #endregion

    #region State Handling
    public override void OnStateEnter() {
        pathFinder.OnDeadzoneMoveStop += EnterAttackState;
    }
    public override void OnStateExit() {
        pathFinder.OnDeadzoneMoveStop -= EnterAttackState;
    }
    public override void Handle() {
        pathFinder.MoveTowardsTarget(playerTransform.position, moveDeadzone);
        animator.SetFloat("Speed", pathFinder.ReturnCurrentSpeed());
    }
    #endregion

    #region Helper Methods
    void EnterAttackState() {
        Debug.Log("Tried to enter attack state.");
        //owner.SwitchState(targetState);
    }
    #endregion
}
