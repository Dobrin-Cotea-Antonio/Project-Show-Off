using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : ClimbableObject {

    [Header("Climb Speed")]
    [SerializeField] [Range(0, 10)] float sideMoveSpeed;
    [SerializeField] [Range(0, 10)] float climbSpeed;

    #region Movement Resolve
    protected override void ResolveMovement() {
        switch (armsOnThisObject) {
            case 0:
                ///Do nothing
                break;
            case 1:
                ///Move when the unplaced hand moves (move to the side)
                break;
            case 2:
                ///Move up when both hands are placed;
                //MoveUp();
                MoveToTheSide();
                break;
        }
    }
    #endregion

    #region Movement

    private void MoveToTheSide() {

        Vector3 movement = Vector3.zero;

        Vector3 arm1Movement = player.armMovementThisFrame[0];
        //arm1Movement.y = 0;

        Vector3 arm2Movement = player.armMovementThisFrame[1];
        //arm2Movement.y = 0;

        if (Mathf.Abs(arm1Movement.magnitude) > Mathf.Abs(arm2Movement.magnitude))
            movement = arm1Movement;
        else
            movement = arm2Movement;

        Debug.Log(movement);

        playerController.Move(new Vector3(movement.z, -movement.y, movement.x));
    }

    private void MoveUp() {
        Vector3 movement = Vector3.zero;

        if (player.armMovementThisFrame[0].y > player.armMovementThisFrame[1].y)
            movement = player.armMovementThisFrame[0];
        else
            movement = player.armMovementThisFrame[1];

        //maxMovement = new Vector3(0, -maxMovement.y, 0);

        movement = new Vector3(0, -movement.y * climbSpeed, 0);

        //playerController.Move(maxMovement * climbSpeed);

        playerController.Move(movement);
    }
    #endregion
}
