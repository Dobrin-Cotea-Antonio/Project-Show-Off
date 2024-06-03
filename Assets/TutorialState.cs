using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState : GameState {

    [Header("Time Data")]
    [SerializeField] float timeUntilEnemyShipComesIn;

    [Header("Target State")]
    [SerializeField] GameStateID targetState;

    float timePassed;

    #region State Handling
    public override void OnStateEnter() {
        timePassed = 0;
    }

    public override void OnStateExit() {
        timePassed = 0;
    }

    public override void Handle() {
        timePassed += Time.deltaTime;

        if (timePassed >= timeUntilEnemyShipComesIn) {
            Debug.Log(timePassed + " " + timeUntilEnemyShipComesIn);
            SceneManager.instance.SwitchState(targetState);
        }
    }
    #endregion


}
