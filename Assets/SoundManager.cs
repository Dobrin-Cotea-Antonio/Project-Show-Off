using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        Background,
        BackgroundCombat,
        ShipAudio,
        WoodCreaking,
        WindBlowing,
        SailFlapping,
        Seagulls,
        Waves,
        PirateYell,
        Rope,
        Cutting,
        Shooting,
        VoiceLine,
        SawingMast,
        CannonFire,
        ButtonHover,
    }
    
    private static Dictionary<Sound, AudioSource> activeSounds = new Dictionary<Sound, AudioSource>();

    public static void PlaySound(Sound sound, Transform position)
    {
        AudioSource audioSource = AudioSourcePool.instance.GetAudioSource();
        GameAssets.SoundSettings settings = GetSoundSettings(sound);

        if (settings != null)
        {
            audioSource.transform.position = position.position;
            audioSource.outputAudioMixerGroup = settings.mixerGroup;
            audioSource.playOnAwake = settings.playOnAwake;
            audioSource.mute = settings.mute;
            audioSource.loop = settings.loop;
            audioSource.clip = settings.GetRandomClip();
            audioSource.volume = settings.volume;
            audioSource.pitch = settings.pitch;
            audioSource.spatialBlend = settings.spatialBlend;
            audioSource.dopplerLevel = settings.dopplerLevel;
            audioSource.reverbZoneMix = settings.reverbZoneMix;
            audioSource.rolloffMode = settings.volumeRolloff;
            audioSource.minDistance = settings.minDistance;
            audioSource.maxDistance = settings.maxDistance;
            audioSource.Play();
            
            Debug.Log($"Playing {sound} at {position.position}");

            activeSounds[sound] = audioSource;
            
            AudioSourcePool.instance.StartCoroutine(ReturnToPoolAfterPlaying(audioSource, sound));

        }
        else
        {
            Debug.LogError("Sound settings for " + sound + " not found!");
        }
    }

    public static void PlaySound(Sound sound)
    {
        AudioSource audioSource = AudioSourcePool.instance.GetAudioSource();
        GameAssets.SoundSettings settings = GetSoundSettings(sound);

        if (settings != null)
        {
            audioSource.pitch = settings.pitch;
            audioSource.outputAudioMixerGroup = settings.mixerGroup;
            audioSource.playOnAwake = settings.playOnAwake;
            audioSource.mute = settings.mute;
            audioSource.loop = settings.loop;
            audioSource.clip = settings.GetRandomClip();
            audioSource.volume = settings.volume;
            audioSource.Play();
            
            activeSounds[sound] = audioSource;
            
            AudioSourcePool.instance.StartCoroutine(ReturnToPoolAfterPlaying(audioSource, sound));
        }
        else
        {
            Debug.LogError("Sound settings for " + sound + " not found!");
        }
    }

    private static GameAssets.SoundSettings GetSoundSettings(Sound sound)
    {
        foreach (var settings in GameAssets.instance.soundSettings)
        {
            if (settings.sound == sound)
            {
                return settings;
            }
        }

        return null;
    }
    
    public static void StopSound(Sound sound)
    {
        if (activeSounds.TryGetValue(sound, out AudioSource audioSource))
        {
            audioSource.Stop();
            AudioSourcePool.instance.ReturnAudioSource(audioSource);
            activeSounds.Remove(sound);

            Debug.Log($"Stopping {sound}");
        }
        else
        {
            Debug.LogWarning($"No active sound found for {sound}");
        }
    }
    
    private static IEnumerator ReturnToPoolAfterPlaying(AudioSource audioSource, Sound sound)
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        if (activeSounds.ContainsKey(sound) && activeSounds[sound] == audioSource)
        {
            activeSounds.Remove(sound);
            Debug.Log($"Returned {sound} to pool");
        }
        AudioSourcePool.instance.ReturnAudioSource(audioSource);
    }
}