using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComingInState : GameState {

    [Header("Enemy Ship")]
    [SerializeField] EnemyShipScript enemyShip;

    [Header("Target State")]
    [SerializeField] GameStateID targetState;

    #region State Handling
    public override void OnStateEnter() {
        enemyShip.EnableMovement(true);
        enemyShip.OnBoardFinish += OnShipArrival;
        
        SoundManager.PlaySound(SoundManager.Sound.BackgroundCombat);
    }

    public override void OnStateExit() {
        enemyShip.EnableMovement(false);
        enemyShip.OnBoardFinish -= OnShipArrival;
    }

    public override void Handle() {

    }
    #endregion

    #region Helper Methods
    private void OnShipArrival() {
        SceneManager.instance.SwitchState(targetState);
    }
    #endregion
}
