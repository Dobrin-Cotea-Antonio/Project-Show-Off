using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    public static SceneManager instance { get; private set; }
    public GameObject playerGameObject { get; private set; }

    [Header("Player Info")]
    [SerializeField] bool useVRController = true;

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
}
