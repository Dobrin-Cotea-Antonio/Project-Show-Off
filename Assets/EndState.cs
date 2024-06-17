using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EndState : GameState {

    [Header("End Data")]
    [SerializeField] XRSimpleInteractable gameEndInteractable;

    #region State Handling
    public override void Handle() {

    }

    public override void OnStateEnter() {

    }

    public override void OnStateExit() {

    }
    #endregion

    #region Helper Methods
    private void GoToEndScreen() { 
    
    }
    #endregion
}
