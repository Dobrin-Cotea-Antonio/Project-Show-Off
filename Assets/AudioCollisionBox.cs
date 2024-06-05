using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCollisionBox : MonoBehaviour
{
    [Header("Target State")]
    [SerializeField] GameStateID targetState;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SoundManager.PlaySound(SoundManager.Sound.CannonFire);
            gameObject.gameObject.SetActive(false);
            SceneManager.instance.SwitchState(targetState);
        }
    }
    
    
}
