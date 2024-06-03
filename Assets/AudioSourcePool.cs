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
                GameObject go = new GameObject("AudioSourcePool");
                _instance = go.AddComponent<AudioSourcePool>();
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
            AudioSource source = availableAudioSources[0];
            availableAudioSources.RemoveAt(0);
            inUseAudioSources.Add(source);
            source.gameObject.SetActive(true);
            return source;
        }
        else
        {
            GameObject go = new GameObject("PooledAudioSource");
            AudioSource source = go.AddComponent<AudioSource>();
            source.transform.SetParent(transform);
            inUseAudioSources.Add(source);
            return source;
        }
    }
    
    public void ReturnAudioSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        inUseAudioSources.Remove(source);
        availableAudioSources.Add(source);
    }

    private void Update()
    {
        for (int i = inUseAudioSources.Count - 1; i >= 0; i--)
        {
            if(!inUseAudioSources[i].isPlaying)
                ReturnAudioSource(inUseAudioSources[i]);
        }
    }
}
