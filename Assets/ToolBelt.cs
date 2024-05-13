using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolBelt : MonoBehaviour {
    [SerializeField] CharacterController characterController;
    [SerializeField] [Tooltip("Percentage of the player height this will be placed at")] float heightPercentage;
    //[SerializeField] Vector3 _attachPosition;

    //public Vector3 attachPosition { get { return _attachPosition; } }
    Vector3 initialOffset;

    #region Unity Events
    private void Awake() {
        initialOffset = transform.localPosition;
        initialOffset.y = 0;
    }

    private void Update() {
        transform.localPosition = new Vector3(initialOffset.x + characterController.center.x, characterController.height * (heightPercentage / 100f), initialOffset.z + characterController.center.z);

        //if (attachedTransform != null)
        //   attachedTransform.localPosition = transform.localPosition;
    }

    private void OnTriggerEnter(Collider pOther) {
        IAttachable attachable = pOther.GetComponent<IAttachable>();

        if (attachable == null)
            return;

        if (attachable.AttachedToolbelt() != null)
            return;

        //Debug.Log("entered");
        attachable.Attach(this);
    }

    private void OnTriggerExit(Collider pOther) {
        IAttachable attachable = pOther.GetComponent<IAttachable>();

        if (attachable == null)
            return;

        if (attachable.AttachedToolbelt() != this)
            return;

        //Debug.Log("exited");

        attachable.Detach(this);
    }
    #endregion
}
