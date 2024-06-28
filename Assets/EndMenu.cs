using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour
{
    [Header("Game End Sprites")] [SerializeField]
    SpriteRenderer winSpriteRenderer;

    [SerializeField] SpriteRenderer loseSpriteRenderer;

    private void Start()
    {
        bool hasPlayerWon = GameWinState.instance.hasPlayerWon;
        if (hasPlayerWon)
        {
            winSpriteRenderer.enabled = true;
            loseSpriteRenderer.enabled = false;
        }
        else
        {
            loseSpriteRenderer.enabled = true;
            winSpriteRenderer.enabled = false;
        }
    }
}