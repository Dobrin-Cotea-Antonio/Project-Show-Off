using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : GameState {

    [Header("Scene Transition")]
    [SerializeField] string endScene;

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

    public override void Handle() {

    }
    #endregion

    #region Helper Methods
    private void SwitchToDeathScene() {
        GameWinState.instance.hasPlayerWon = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(endScene);
    }

    private void SwitchToWinScreen(int pWave, int pMaxWave) {
        if (pWave != pMaxWave)
            return;

        GameWinState.instance.hasPlayerWon = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(endScene);
    }
    #endregion
}
