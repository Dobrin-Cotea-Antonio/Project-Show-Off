using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateID { 
    TutorialState,
    ShipComingInState,
    GameplayState,
    EndState
}

public abstract class GameState : MonoBehaviour {

    [Header("State Data")]
    [SerializeField] private GameStateID _stateID;
    public GameStateID stateID { get { return _stateID; } }

    #region State Handling
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void Handle();
    #endregion
}
