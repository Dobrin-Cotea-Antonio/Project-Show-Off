using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Rope : MonoBehaviour {

    [Header("Climb Speed")]

    Vector3 lastGrabPoint = Vector3.zero;

    #region Unity Events
    #endregion

    #region Climbing
    private void Climb(SelectEnterEventArgs pArgs) {

        Debug.Log("Test");

        //player.armsInUse++;

        Vector3 grabPoint = pArgs.interactorObject.transform.position;
        Vector3 moveDistance = Vector3.zero;

        if (lastGrabPoint != Vector3.zero) {
            moveDistance = new Vector3(0, grabPoint.y - lastGrabPoint.y);
            Debug.Log(moveDistance);
        }


    }
    #endregion
}
