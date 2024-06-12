using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class ClimbableObject : MonoBehaviour {

    [Header("Constraints")]
    [SerializeField] protected bool canMoveX;
    [SerializeField] protected bool canMoveY;
    [SerializeField] protected bool canMoveZ;

    protected XRSimpleInteractable interactable;
    protected CharacterController playerController;
    protected Player player;

    protected int armsOnThisObject = 0;
    protected XRDirectInteractor lastHandInteractedWith = null;
    protected int lastHandInteractedWithIndex = -1;

    #region Unity Events
    protected virtual void Awake() {
        interactable = GetComponent<XRSimpleInteractable>();
        //interactable.selectEntered.AddListener(SelectEnter);
        //interactable.selectExited.AddListener(SelectExit);
    }

    protected virtual void Start() {
        playerController = SceneManager.instance.playerGameObject.GetComponent<CharacterController>();
        player = playerController.GetComponent<Player>();
    }

    private void LateUpdate() {
        ClearHandInteractedWith();
        ResolveMovement();
    }
    #endregion

    #region Helper Methods
    public virtual void SelectEnter(XRDirectInteractor pHand, int pIndex) { }

    public virtual void SelectExit(XRDirectInteractor pHand,int pIndex) { }

    public void ChangeArmUseValue(bool pIncreaseValue) {
        armsOnThisObject = (pIncreaseValue) ? armsOnThisObject + 1 : armsOnThisObject - 1;
    }

    public void SetLastArmInteractedWith(XRDirectInteractor pHand, int pIndex) {
        lastHandInteractedWith = pHand;
        lastHandInteractedWithIndex = pIndex;
    }

    private void ClearHandInteractedWith() {
        if (armsOnThisObject != 0)
            return;
    
        lastHandInteractedWith = null;
        lastHandInteractedWithIndex = -1;
    }
    #endregion

    #region Movement Resolve
    protected abstract void ResolveMovement();
    #endregion
}
