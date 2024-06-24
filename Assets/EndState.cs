using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class EndState : GameState {

    [Header("End Data")]
    [SerializeField] XRSimpleInteractable gameEndInteractable;
    [SerializeField] string endScene;
    [SerializeField] MastScript mast;

    #region Unity Events
    private void Awake(){
        mast.OnDestroy += LoseGame;
    }
    #endregion

    #region State Handling
    public override void Handle() {}

    public override void OnStateEnter() {
        gameEndInteractable.selectEntered.AddListener(GoToEndScreen);
        // play voicelines
        SoundManager.PlaySoundRepeating(SoundManager.Sound.VoiceLine_PLAYER_WON, 10);
    }

    public override void OnStateExit() {
        gameEndInteractable.selectEntered.RemoveListener(GoToEndScreen);
        
        // stop voicelines
        SoundManager.StopSound(SoundManager.Sound.VoiceLine_PLAYER_WON);
    }
    #endregion

    #region Helper Methods
    private void GoToEndScreen(SelectEnterEventArgs pArgs) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(endScene);
    }

    private void LoseGame() {
        GameWinState.instance.hasPlayerWon = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(endScene);
    }
    #endregion
}
