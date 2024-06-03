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
    }

    private static Dictionary<AudioSource, Sound> playingAudioSources = new Dictionary<AudioSource, Sound>();

    static SoundManager()
    {
        GameAssets.instance.OnSoundSettingsChanged += UpdateSound;
    }

    private static void UpdateSound(Sound sound)
    {
        foreach (var kvp in playingAudioSources)
        {
            if (kvp.Value == sound)
            {
                UpdateAudioSourceSettings(kvp.Key, sound);
            }
        }
    }

    private static void UpdateAudioSourceSettings(AudioSource audioSource, Sound sound)
    {
        GameAssets.SoundSettings settings = GetSoundSettings(sound);
        if(settings != null)
        {
            audioSource.volume = settings.volume;
            audioSource.pitch = settings.pitch;
            audioSource.spatialBlend = settings.spatialBlend;
            audioSource.dopplerLevel = settings.dopplerLevel;
            audioSource.reverbZoneMix = settings.reverbZoneMix;
            audioSource.rolloffMode = settings.volumeRolloff;
            audioSource.minDistance = settings.minDistance;
            audioSource.maxDistance = settings.maxDistance;
        }
    }

    public static void PlaySound(Sound sound, Transform position)
    {
        AudioSource audioSource = AudioSourcePool.instance.GetAudioSource();
        GameAssets.SoundSettings settings = GetSoundSettings(sound);

        if (settings != null)
        {
            audioSource.transform.position = position.position;
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

            AudioSourcePool.instance.StartCoroutine(ReturnToPoolAfterPlaying(audioSource));

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
            audioSource.PlayOneShot(settings.GetRandomClip(), settings.volume);
            audioSource.pitch = settings.pitch;
            
            AudioSourcePool.instance.StartCoroutine(ReturnToPoolAfterPlaying(audioSource));
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
    
    private static IEnumerator ReturnToPoolAfterPlaying(AudioSource audioSource)
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        AudioSourcePool.instance.ReturnAudioSource(audioSource);
    }
}