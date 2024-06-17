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
        
        // play background combat sound
        SoundManager.PlaySound(SoundManager.Sound.BackgroundCombat);
        
        // play cannon sound
        SoundManager.PlaySound(SoundManager.Sound.CannonFire);
        
        // play voice lines
        SoundManager.PlaySoundRepeating(SoundManager.Sound.VoiceLine_PLAYER_ENEMY_INCOMING, 5);
    }

    public override void OnStateExit() {
        enemyShip.EnableMovement(false);
        enemyShip.OnBoardFinish -= OnShipArrival;
        
        // stop voice lines
        SoundManager.StopSound(SoundManager.Sound.VoiceLine_PLAYER_ENEMY_INCOMING);
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
