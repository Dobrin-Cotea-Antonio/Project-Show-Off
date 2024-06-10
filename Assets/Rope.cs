using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Rope : MonoBehaviour {

    [Header("Climb Speed")]
    //[SerializeField] Vector3 speed;

    XRSimpleInteractable interactable;
    CharacterController playerController;

    Vector3 lastGrabPoint;
    int interactorCount = 0;

    #region Unity Events
    private void Awake() {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(Climb);
    }
    #endregion

    #region Climbing
    private void Climb(SelectEnterEventArgs pArgs) {
        if (playerController == null)
            playerController = SceneManager.instance.playerGameObject.GetComponent<CharacterController>();

        interactorCount++;

        //playerController.Move(speed);
    }

    private void DecreaseInteractorCount(SelectExitEventArgs pArgs) {
        interactorCount--;
    }
    #endregion
}
