using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialState : GameState {

    [Header("Time Data")]
    [SerializeField] float timeUntilEnemyShipComesIn;

    [Header("Target State")]
    [SerializeField] GameStateID targetState;

    [Header("Action Sequence")]
    [SerializeField] PistolScript pistol;

    XRGrabInteractable pistolInteractableComponent;
    XRSimpleInteractable hookInteractableComponent;

    int stage = 0;
    float timePassed;

    #region Unity Events
    private void Awake() {
        pistolInteractableComponent = pistol.GetComponent<XRGrabInteractable>();
        hookInteractableComponent = pistol.GetComponent<XRSimpleInteractable>();
    }
    #endregion

    #region State Handling
    public override void OnStateEnter() {
        //if (stage == 0) {
        //    pistol.GetComponent<Outline>().enabled = true;
        //    pistolInteractableComponent.selectEntered.AddListener(DisablePistolHighlight);
        //}

        timePassed = 0;
    }

    public override void OnStateExit() {
        //pistolInteractableComponent.selectEntered.RemoveListener(DisablePistolHighlight);
        timePassed = 0;
    }

    public override void Handle() {

        //switch (stage) {
        //    case 0://highlight pistol

        //        break;
        //    case 1://highlight rope

        //        break;
        //    case 2://grab rope

        //        break;
        //    case 3://moving on the rope

        //        break;
        //    case 4://got up

        //        break;

        //}

        timePassed += Time.deltaTime;

        if (timePassed >= timeUntilEnemyShipComesIn) {
            Debug.Log(timePassed + " " + timeUntilEnemyShipComesIn);
            SceneManager.instance.SwitchState(targetState);
        }
    }
    #endregion

    #region Highlight
    void DisablePistolHighlight(SelectEnterEventArgs pArgs) {
        if (stage != 0)
            return;

        stage++;
        pistol.GetComponent<Outline>().enabled = false;
        pistolInteractableComponent.selectEntered.RemoveListener(DisablePistolHighlight);

        SceneManager.instance.hook.GetComponent<Outline>().enabled = true;
        SceneManager.instance.hook.OnMastReached += DisableHookHighlight;
    }

    void DisableHookHighlight() {
        if (stage != 1)
            return;

        SceneManager.instance.hook.GetComponent<Outline>().enabled = false;

    }
    #endregion
}
