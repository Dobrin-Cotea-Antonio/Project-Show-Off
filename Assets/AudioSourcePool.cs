using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    private static AudioSourcePool _instance;
    public static AudioSourcePool instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject audioSourcePool = new GameObject("AudioSourcePool");
                _instance = audioSourcePool.AddComponent<AudioSourcePool>();
            }
            return _instance;
        }
    }
    
    private List<AudioSource> availableAudioSources = new List<AudioSource>();
    private List<AudioSource> inUseAudioSources = new List<AudioSource>();

    public AudioSource GetAudioSource()
    {
        if (availableAudioSources.Count > 0)
        {
            AudioSource audioSource = availableAudioSources[0];
            availableAudioSources.RemoveAt(0);
            inUseAudioSources.Add(audioSource);
            audioSource.gameObject.SetActive(true);
            return audioSource;
        }
        else
        {
            GameObject obj = new GameObject("PooledAudioSource");
            AudioSource audioSource = obj.AddComponent<AudioSource>();
            inUseAudioSources.Add(audioSource);
            return audioSource;
        }
    }
    
    public void ReturnAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.gameObject.SetActive(false);
        inUseAudioSources.Remove(audioSource);
        availableAudioSources.Add(audioSource);
    }

    private void Update()
    {
        for (int i = inUseAudioSources.Count - 1; i >= 0; i--)
        {
            if (!inUseAudioSources[i].isPlaying)
            {
                ReturnAudioSource(inUseAudioSources[i]);
            }
        }
    }
}
