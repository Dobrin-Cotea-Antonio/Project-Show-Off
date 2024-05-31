using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    Playing, 
    PlayerWon,
    PlayerDied,
}

public class SceneManager : MonoBehaviour {
    public static SceneManager instance { get; private set; }
    public GameObject playerGameObject { get; private set; }

    [Header("Player Info")]
    [SerializeField] bool useVRController = true;
    
    [Header("Timing Info")]
    [SerializeField] float timeToWaitBeforeShipInvasion = 5f;
    
    private GameState currentState;

    public event Action<GameState> OnStateChanged; 

    [SerializeField] GameObject vrControllerPrefab;
    [SerializeField] GameObject fpsControllerPrefab;

    [Header("Player Spawning")]
    [SerializeField] [Tooltip("Rotate the spawnpoint in the direction the player should be facing when he spawns")] Transform playerSpawnPoint;

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
    }
    #endregion
    
    private void Start()
    {
        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
        HandleStateChange(currentState);
    }

    private void HandleStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                EventManager.instance.StartShipInvasion(timeToWaitBeforeShipInvasion);
                break;
            case GameState.PlayerWon:
                break;
            case GameState.PlayerDied:
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    public void PlayerDied()
    {
        ChangeState(GameState.PlayerDied);
    }

    public void PlayerWon()
    {
        ChangeState(GameState.PlayerWon);
    }
}
