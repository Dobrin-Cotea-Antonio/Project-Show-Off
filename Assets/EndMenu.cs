using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour {
    [Header("Game End Sprites")]
    [SerializeField] SpriteRenderer winSpriteRenderer; 
    [SerializeField] SpriteRenderer loseSpriteRenderer;

    private void Start() {
        bool hasPlayerWon = GameWinState.instance.hasPlayerWon;
        
        winSpriteRenderer.enabled = hasPlayerWon;
        loseSpriteRenderer.enabled = !hasPlayerWon;
    }
}
