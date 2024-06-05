using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    public SoundManager.Sound sound;
    [SerializeField] private bool is3Dsound = false;
    [SerializeField] private bool isPlayingAtRandom = false;
    [SerializeField] private float minTime = 1f;
    [SerializeField] private float maxTime = 5f;

    private void Start()
    {
        if (isPlayingAtRandom)
        {
            StartCoroutine(PlaySoundAtRandomIntervals());
        }
        else
        {
            PlaySound();
        }
    }

    private void PlaySound()
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

    private IEnumerator PlaySoundAtRandomIntervals()
    {
        while (true)
        {
            float randomTime = UnityEngine.Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randomTime);
            PlaySound();
        }
    }
}
