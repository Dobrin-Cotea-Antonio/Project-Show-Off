using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ShootState : EnemyState {

    [Header("Weapon Data")]
    [SerializeField] EnemyWeapon weapon;

    CharacterController targetCharacterController;

    [Header("Animation")]
    [SerializeField] Animator animator;
    
    //[SerializeField] RigBuilder

    #region Unity Events
    private void Start() {
        targetCharacterController = SceneManager.instance.playerGameObject.GetComponent<CharacterController>();
    }
    #endregion

    #region State Handling
    public override void Handle() {
        animator.SetFloat("Speed", 0);
        weapon.Shoot(FindPointToShoot());
    }

    public override void OnStateEnter() { }

    public override void OnStateExit() { }
    #endregion

    #region Helper Methods
    Vector3 FindPointToShoot() {
        return targetCharacterController.transform.position + new Vector3(0, targetCharacterController.height * 0.5f, 0);
    }
    #endregion
}
