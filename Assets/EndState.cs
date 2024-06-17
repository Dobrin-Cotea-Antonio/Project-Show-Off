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

        // play voicelines
        SoundManager.PlaySoundRepeating(SoundManager.Sound.VoiceLine_PLAYER_WON, 10);
    }

    public override void OnStateExit() {

        // stop voicelines
        SoundManager.StopSound(SoundManager.Sound.VoiceLine_PLAYER_WON);
    }
    #endregion

    #region Helper Methods
    private void GoToEndScreen() { 
    
    }
    #endregion
}
