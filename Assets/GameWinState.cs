using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinState : MonoBehaviour {
    public static GameWinState instance { get; private set; }

    public bool hasPlayerWon { get; set; }

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
        hasPlayerWon = false;
    }

}
