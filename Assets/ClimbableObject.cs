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

    #region Unity Events
    protected virtual void Awake() {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(IncreaseInteractorCount);
        interactable.selectExited.AddListener(DecreaseInteractorCount);
    }

    protected virtual void Start() {
        playerController = SceneManager.instance.playerGameObject.GetComponent<CharacterController>();
        player = playerController.GetComponent<Player>();
    }

    private void LateUpdate() {
        ResolveMovement();
    }
    #endregion

    #region Interactor Count
    private void IncreaseInteractorCount(SelectEnterEventArgs pArgs) {
        player.armsInUse++;
    }

    private void DecreaseInteractorCount(SelectExitEventArgs pArgs) {
        player.armsInUse--;
    }
    #endregion

    #region Movement Resolve
    protected abstract void ResolveMovement();
    #endregion

}
