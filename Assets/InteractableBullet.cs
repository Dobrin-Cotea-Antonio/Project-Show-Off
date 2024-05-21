using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;

public class InteractableBullet : MonoBehaviour, IAttachable {
    [Header("Tool Belt")]
    [SerializeField] Vector3 attachRotation;
    ToolBelt toolbeltAttachedTo = null;
    Transform lastHandInteractedWith;

    XRGrabInteractable interactableComponent;

    #region Unity Events
    private void Awake() {
        interactableComponent = GetComponent<XRGrabInteractable>();
        interactableComponent.retainTransformParent = false;
        interactableComponent.selectEntered.AddListener(OnSelect);
        interactableComponent.selectExited.AddListener(OnDeselect);
    }
    #endregion

    #region Attachable
    public void Attach(ToolBelt pBelt) {
        toolbeltAttachedTo = pBelt;
        interactableComponent.selectExited.AddListener(PlaceOnToolbelt);
    }

    public ToolBelt AttachedToolbelt() {
        return toolbeltAttachedTo;
    }

    public void Detach(ToolBelt pBelt) {
        Rigidbody rb = GetComponent<Rigidbody>();
        interactableComponent.selectExited.RemoveListener(PlaceOnToolbelt);

        toolbeltAttachedTo = null;
        transform.parent = lastHandInteractedWith;

        rb.useGravity = true;
    }

    void PlaceOnToolbelt(SelectExitEventArgs pArgs) {
        if (toolbeltAttachedTo == null)
            return;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        transform.parent = toolbeltAttachedTo.transform;
        transform.eulerAngles = attachRotation;
        transform.localPosition = Vector3.zero;

        rb.useGravity = false;
    }

    void OnSelect(SelectEnterEventArgs pArgs) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        lastHandInteractedWith = pArgs.interactorObject.transform;

        if (toolbeltAttachedTo == null)
            transform.parent = pArgs.interactorObject.transform;
    }

    void OnDeselect(SelectExitEventArgs pArgs) {
        if (toolbeltAttachedTo == null)
            transform.parent = null;
    }

    #endregion
}
