using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShootState : EnemyState {

    [Header("Weapon Data")]
    [SerializeField] EnemyWeapon weapon;

    CharacterController targetCharacterController;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] Transform wristTransform;
    [SerializeField] Transform rightArmAimTarget;
    [SerializeField] Transform torsoAimTarget;
    [SerializeField] Transform headAimTarget;
    [SerializeField] Rig rightArmRig;
    [SerializeField] Rig torsoRig;
    [SerializeField] Rig headRig;
    [SerializeField] float armRaiseDuration;

    [Header("Rotation")]
    [SerializeField] float maxAngleToShoot;
    [SerializeField] float rotationSpeed;

    [Header("Sound")]
    [SerializeField] SoundManager.Sound attackSound;

    float rotationEpsilon = 1.5f;

    #region Unity Events
    private void Start() {
        targetCharacterController = SceneManager.instance.playerGameObject.GetComponent<CharacterController>();
        OnStateExit();

        weapon.OnSuccessfulShot += PlaySound;
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

        torsoAimTarget.position = point;
        rightArmAimTarget.position = point;
        headAimTarget.position = point;

        animator.SetFloat("Speed", 0);
        weapon.Shoot(point);

    }

    public override void OnStateEnter() {
        rightArmRig.enabled = true;
        headRig.enabled = true;
        torsoRig.enabled = true;

        rightArmRig.weight = 1;
        headRig.weight = 1;
        torsoRig.weight = 1;
    }

    public override void OnStateExit() {
        rightArmRig.enabled = false;
        headRig.enabled = false;
        torsoRig.enabled = false;

        rightArmRig.weight = 0;
        headRig.weight = 0;
        torsoRig.weight = 0;
    }
    #endregion

    #region Helper Methods
    Vector3 FindPointToShoot() {
        return targetCharacterController.transform.position + new Vector3(0, targetCharacterController.height * 0.8f, 0);
    }

    void PlaySound() {
        Debug.Log("COcK");
        SoundManager.PlaySound(attackSound, transform);
    }
    #endregion
}
