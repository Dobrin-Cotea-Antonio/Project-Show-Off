using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : GameState {

    [Header("Scene Transition")]
    [SerializeField] GameStateID targetState;

    #region State Handling
    public override void OnStateEnter() {
        EnemyManager.instance.Enable(true);
        EnemyManager.instance.OnWaveEnd += SwitchToWinScreen;
        EnemyManager.instance.mastTransform.GetComponent<MastScript>().OnDestroy += SwitchToDeathScene;
    }

    public override void OnStateExit() {
        EnemyManager.instance.Enable(false);
        EnemyManager.instance.OnWaveEnd -= SwitchToWinScreen;
        SceneManager.instance.playerGameObject.GetComponent<Player>().OnDeath -= SwitchToDeathScene;
    }

    public override void Handle() { }
    #endregion

    #region Helper Methods
    private void SwitchToDeathScene() {
        GameWinState.instance.hasPlayerWon = false;
        SceneManager.instance.SwitchState(targetState);
    }

    private void SwitchToWinScreen(int pWave, int pMaxWave) {
        if (pWave != pMaxWave - 1)
            return;

        GameWinState.instance.hasPlayerWon = true;
        SceneManager.instance.SwitchState(targetState);
    }
    #endregion
}
