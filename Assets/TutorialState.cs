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
        hookInteractableComponent = SceneManager.instance.hook.GetComponent<XRSimpleInteractable>();
    }
    #endregion

    #region State Handling
    public override void OnStateEnter() {
        if (stage == 0) {
            SceneManager.instance.hook.GetComponent<ObjectGlow>().Enable(true);
            hookInteractableComponent.selectEntered.AddListener(DisableHookHighlight);
        };
        
        // play background sound
        SoundManager.PlaySound(SoundManager.Sound.Background);
        
        // play voice lines
        SoundManager.PlaySoundRepeating(SoundManager.Sound.VoiceLine_PLAYER_EXPLORES_SHIP, 15);
        
        timePassed = 0;
    }

    public override void OnStateExit() {
        timePassed = 0;
        
        // stop background sound
        SoundManager.StopSound(SoundManager.Sound.Background);
        
        // stop voice lines
        SoundManager.StopSound(SoundManager.Sound.VoiceLine_PLAYER_EXPLORES_SHIP);
    }

    public override void Handle() {
        timePassed += Time.deltaTime;

        if (timePassed >= timeUntilEnemyShipComesIn) {
            Debug.Log(timePassed + " " + timeUntilEnemyShipComesIn);
            SceneManager.instance.SwitchState(targetState);
        }
    }
    #endregion

    #region Highlight
    void DisablePistolHighlight(SelectEnterEventArgs pArgs) {
        /*if (stage != 1)
            return;*/
        
        stage++;

        pistol.GetComponent<ObjectGlow>().Enable(false);
        pistolInteractableComponent.selectEntered.RemoveListener(DisablePistolHighlight);
    }

    void DisableHookHighlight(SelectEnterEventArgs pArgs) {
        if (stage != 0)
            return;

        SceneManager.instance.hook.GetComponent<ObjectGlow>().Enable(false);
        pistol.GetComponent<ObjectGlow>().Enable(true);

        hookInteractableComponent.selectEntered.RemoveListener(DisableHookHighlight);
        pistolInteractableComponent.selectEntered.AddListener(DisablePistolHighlight);
    }
    #endregion
}