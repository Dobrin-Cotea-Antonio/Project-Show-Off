using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Rope : ClimbableObject {

    [Header("Climb Speed")]
    [SerializeField] [Range(0, 10)] float climbSpeed;

    int armNeededIndex;

    #region Movement Resolve
    protected override void ResolveMovement() {
        if (armsOnThisObject == 2)
            MoveUp();
    }
    #endregion

    #region Movement
    private void MoveUp() {
        Vector3 movement = player.armMovementThisFrame[armNeededIndex];

        movement = new Vector3(0, -movement.y * climbSpeed, 0);

        playerController.Move(movement);
    }
    #endregion

    #region Helper Methods
    public override void SelectEnter(XRDirectInteractor pHand, int pIndex) {
        if (armsOnThisObject != 0)
            return;

        armNeededIndex = pIndex;
        CycleArm();

    }

    public override void SelectExit(XRDirectInteractor pHand, int pIndex) {
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