using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneManager : MonoBehaviour {
    public Action<GameState> OnEnterStateOfType;
    public Action<GameState> OnExitStateOfType;

    public static SceneManager instance { get; private set; }
    public GameObject playerGameObject { get; private set; }

    [Header("Player Info")]
    [SerializeField] bool useVRController = true;

    [SerializeField] GameObject vrControllerPrefab;
    [SerializeField] GameObject fpsControllerPrefab;

    [Header("Player Spawning")]
    [SerializeField] [Tooltip("Rotate the spawnpoint in the direction the player should be facing when he spawns")] Transform playerSpawnPoint;

    [Header("Game State")]
    [SerializeField] GameStateID initialState;
    [SerializeField] GameState[] gameStates;

    [SerializeField] GameStateID activeStateID;

    Dictionary<GameStateID, GameState> enemyStateDictionary = new Dictionary<GameStateID, GameState>();
    GameState activeState;

    [Header("End Scene")]
    [SerializeField] string endScene;

    //[Header("Player State Management")]
    //[SerializeField] 
    //player state

    public bool isPlayerOnMast { get; private set; }



    #region Unity Events
    private void Awake() {
        if (instance != null) {
            Destroy(instance.gameObject);
        }

        instance = this;

        if (useVRController)
            playerGameObject = Instantiate(vrControllerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        else
            playerGameObject = Instantiate(fpsControllerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);

        playerGameObject.GetComponent<Player>().OnDeath += SwitchToDeathScene;

        foreach (GameState state in gameStates)
            enemyStateDictionary[state.stateID] = state;
    }

    private void Start() {
        SwitchState(initialState);
    }

    private void Update() {
        activeState.Handle();
        activeStateID = activeState.stateID;
    }
    #endregion

    #region State Machine
    public void SwitchState(GameState pState) {
        if (pState == activeState)
            return;

        OnExitStateOfType?.Invoke(activeState);

        if (activeState != null)
            activeState.OnStateExit();

        activeState = pState;

        activeState.OnStateEnter();

        OnEnterStateOfType?.Invoke(activeState);
    }

    public void SwitchState(GameStateID pStateID) {
        SwitchState(enemyStateDictionary[pStateID]);
    }
    #endregion

    #region Helper Methods
    private void SwitchToDeathScene() {
        GameWinState.instance.hasPlayerWon = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(endScene);
    }
    #endregion
}

