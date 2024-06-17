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
        
        // play enemy voice lines
        SoundManager.PlaySoundRepeating(SoundManager.Sound.VoiceLine_ENEMY_INCOMING, Random.Range(5,10), transform);
    }
    public override void OnStateExit() {
        pathFinder.OnDeadzoneMoveStop -= EnterAttackState;
        
        // stop enemy voice lines
        SoundManager.StopSound(SoundManager.Sound.VoiceLine_ENEMY_INCOMING);
    }
    public override void Handle() {
        pathFinder.MoveTowardsTarget(playerTransform.position, moveDeadzone);
        animator.SetFloat("Speed", pathFinder.ReturnCurrentSpeed());
    }
    #endregion

    #region Helper Methods
    void EnterAttackState() {
        owner.SwitchState(targetState);
    }
    #endregion
}
