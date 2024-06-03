using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour {
    [Header("Game End Message")]
    [SerializeField] TextMeshPro gameEndText;
    [SerializeField] string winMessage;
    [SerializeField] string loseMessage;

    private void Start() {
        gameEndText.text = (GameWinState.instance.hasPlayerWon) ? winMessage : loseMessage;
    }
}
