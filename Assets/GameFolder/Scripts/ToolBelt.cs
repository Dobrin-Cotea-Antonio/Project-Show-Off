using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolBelt : MonoBehaviour {
    [SerializeField] CharacterController characterController;
    [SerializeField] Camera playerCamera;
    [SerializeField] [Tooltip("Percentage of the player height this will be placed at")] float heightPercentage;
    Vector3 initialOffset;

    #region Unity Events
    private void Awake() {
        initialOffset = transform.localPosition;
        initialOffset.y = 0;
    }

    private void Update() {
        transform.localPosition = new Vector3(initialOffset.x + characterController.center.x, characterController.height * (heightPercentage / 100f), initialOffset.z + characterController.center.z);
        transform.RotateAround(characterController.transform.localPosition, new Vector3(0, 1, 0), (playerCamera.transform.localEulerAngles - transform.localEulerAngles).y);
        transform.localEulerAngles = playerCamera.transform.localEulerAngles;
    }

    private void OnTriggerEnter(Collider pOther) {
        IAttachable attachable = pOther.GetComponent<IAttachable>();

        if (attachable == null)
            return;

        if (attachable.AttachedToolbelt() != null)
            return;

        attachable.Attach(this);
    }

    private void OnTriggerExit(Collider pOther) {
        IAttachable attachable = pOther.GetComponent<IAttachable>();

        if (attachable == null)
            return;

        if (attachable.AttachedToolbelt() != this)
            return;

        attachable.Detach(this);
    }
    #endregion
}
