using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThrowAxeState : EnemyState {

    [Header("Weapon Data")]
    [SerializeField] AxeScript weapon;

    CharacterController targetCharacterController;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] Transform wristTransform;
    [SerializeField] Transform aimTarget;
    [SerializeField] Rig rig;


    [Header("Rotation")]
    [SerializeField] float maxAngleToShoot;
    [SerializeField] float rotationSpeed;

    float rotationEpsilon = 1.5f;

    #region Unity Events
    private void Start() {
        targetCharacterController = SceneManager.instance.playerGameObject.GetComponent<CharacterController>();
        OnStateExit();
    }
    #endregion

    #region State Handling
    public override void Handle() {

        Vector3 targetPos = targetCharacterController.transform.position;
        targetPos.y = transform.position.y;

        Vector3 desiredForwardVector = (targetPos - transform.position).normalized;

        Vector3 forward = transform.forward;

        float angle = Vector3.Angle(forward, desiredForwardVector);

        if (angle > rotationEpsilon) {
            forward = Vector3.RotateTowards(forward, desiredForwardVector, rotationSpeed * Time.deltaTime, 0f);
            transform.forward = forward;
        }

        if (angle > maxAngleToShoot)
            return;//could also lower the arm 

        Vector3 point = FindPointToShoot();

        //aimTarget.position = weapon.ReturnMaxThrowHeight();
        aimTarget.position = point;
        aimTarget.forward = (point - wristTransform.position).normalized;
        animator.SetFloat("Speed", 0);
        //weapon.Shoot(point);
        animator.SetBool("IsThrowing", true);
    }
    
    public void ThrowAxe() {
        weapon.Shoot(FindPointToShoot());
    }

    public override void OnStateEnter() {
        rig.enabled = true;
        rig.weight = 1;
    }

    public override void OnStateExit() {
        rig.enabled = false;
        rig.weight = 0;
        
        animator.SetBool("IsThrowing", false);
    }
    #endregion

    #region Helper Methods
    Vector3 FindPointToShoot() {
        return targetCharacterController.transform.position + new Vector3(0, targetCharacterController.height * 0.8f, 0);
    }
    #endregion
}
