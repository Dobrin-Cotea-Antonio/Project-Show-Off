using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.PlaySound(SoundManager.Sound.Shooting);
        }
    }
}
