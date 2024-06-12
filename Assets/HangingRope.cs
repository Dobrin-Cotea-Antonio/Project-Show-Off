using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingRope : ClimbableObject {

    [Header("Climb Speed")]
    [SerializeField] [Range(0, 10)] float moveSpeed;

    protected override void ResolveMovement() {

        Vector3 forwardDirection = player.transform.forward;

        //rotate player forward direction by the rotation of the X rotation of the rope

    }
}
