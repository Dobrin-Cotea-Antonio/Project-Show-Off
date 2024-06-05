using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    public SoundManager.Sound sound;
    [SerializeField] private bool is3Dsound = false;

    private void Start()
    {
        if (is3Dsound)
        {
            SoundManager.PlaySound(sound, transform);
        }
        else
        {
            SoundManager.PlaySound(sound);
        }
    }
}
