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
    }

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void PlaySound(Sound sound, Transform position)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        GameAssets.SoundSettings settings = GetSoundSettings(sound);

        if (settings != null)
        {
            soundGameObject.transform.position = position.position;
            audioSource.clip = settings.GetRandomClip();
            audioSource.volume = settings.volume * GetCategoryVolume(settings.category);
            audioSource.pitch = settings.pitch;
            audioSource.spatialBlend = settings.spatialBlend;
            audioSource.Play();
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
        else
        {
            Debug.LogError("Sound settings for " + sound + " not found!");
        }
    }

    public static void PlaySound(Sound sound)
    {
        if (oneShotGameObject == null)
        {
            oneShotGameObject = new GameObject("One Shot Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
        }

        GameAssets.SoundSettings settings = GetSoundSettings(sound);

        if (settings != null)
        {
            oneShotAudioSource.PlayOneShot(settings.GetRandomClip(), settings.volume * GetCategoryVolume(settings.category));
            oneShotAudioSource.pitch = settings.pitch;
        }
        else
        {
            Debug.LogError("Sound settings for " + sound + " not found!");
        }
    }
    
    private static float GetCategoryVolume(GameAssets.SoundCategory category)
    {
        foreach (var settings in GameAssets.instance.soundSettings)
        {
            if (settings.category == category)
            {
                return settings.volume;
            }
        }

        return 1f;
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
}