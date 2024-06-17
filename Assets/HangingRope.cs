using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HangingRope : ClimbableObject {

    [Header("Climb Speed")]
    [SerializeField] [Range(0, 10)] private float moveSpeed;

    private Camera playerCamera;

    Vector3 cameraForwardOnEnter;

    int armNeededIndex;

    #region Unity Events
    protected override void Start() {
        base.Start();
        playerCamera = Camera.main;
    }
    #endregion

    #region Movement Resolve
    protected override void ResolveMovement() {
        if (armsOnThisObject != 2)
            return;

        Vector3 movement = player.armMovementThisFrame[armNeededIndex];
        Vector3 forwardDirection = transform.forward;

        if (Vector3.Angle(transform.forward, cameraForwardOnEnter) > 90)
            forwardDirection *= -1;

        playerController.Move(forwardDirection * movement.magnitude * moveSpeed);
    }
    #endregion

    #region Helper Methods

    public override void SelectEnter(XRDirectInteractor pHand, int pIndex) {
        cameraForwardOnEnter = playerCamera.transform.forward;

        if (armsOnThisObject != 0)
            return;

        armNeededIndex = pIndex;
        CycleArm();
    }

    public override void SelectExit(XRDirectInteractor pHand, int pIndex) {
        cameraForwardOnEnter = Vector3.zero;

        if (armsOnThisObject == 0)
            armNeededIndex = -1;
        else
            CycleArm();
    }

    private void CycleArm() {
        if (armNeededIndex == -1)
            return;

        if (armNeededIndex == 0)
            armNeededIndex = 1;
        else
            armNeededIndex = 0;
    }
    #endregion
}