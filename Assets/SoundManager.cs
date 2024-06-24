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
        Seagulls,
        Shooting,
        SawingMast,
        CannonFire,
        ButtonHover,
        VoiceLine_ENEMY_INCOMING,
        VoiceLine_ENEMY_CUTTING_MAST,
        VoiceLine_PLAYER_CLIMBS_MAST,
        VoiceLine_PLAYER_EXPLORES_SHIP,
        VoiceLine_PLAYER_ENEMY_INCOMING,
        VoiceLine_PLAYER_PICKS_UP_WEAPON,
        VoiceLine_PLAYER_ENEMY_CUTTING_MAST,
        VoiceLine_PLAYER_WON,
        VoiceLine_PLAYER_INTERACTS,
    }

    private static Dictionary<Sound, AudioSource> activeSounds = new Dictionary<Sound, AudioSource>();
    private static Dictionary<Sound, Coroutine> repeatingSounds = new Dictionary<Sound, Coroutine>();

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
            audioSource.spatialBlend = settings.spatialBlend;
            audioSource.Play();

            activeSounds[sound] = audioSource;

            AudioSourcePool.instance.StartCoroutine(ReturnToPoolAfterPlaying(audioSource, sound));
        }
        else
        {
            Debug.LogError("Sound settings for " + sound + " not found!");
        }
    }

    public static void PlaySoundRepeating(Sound sound, float interval, Transform position = null)
    {
        if (repeatingSounds.ContainsKey(sound))
        {
            Debug.LogWarning($"Sound {sound} is already playing repeating");
            return;
        }

        Coroutine coroutine = AudioSourcePool.instance.StartCoroutine(PlaySoundRepeatingCoroutine(sound, interval, position));
        repeatingSounds[sound] = coroutine;
    }

    private static IEnumerator PlaySoundRepeatingCoroutine(Sound sound, float interval, Transform position)
    {
        while (true)
        {
            if (position != null)
            {
                PlaySound(sound, position);
            }
            else
            {
                PlaySound(sound);
            }
            float clipLength = GetClipLength(sound);
            yield return new WaitForSeconds(clipLength + interval);
        }
    }

    private static float GetClipLength(Sound sound)
    {
        GameAssets.SoundSettings settings = GetSoundSettings(sound);
        if (settings != null)
        {
            AudioClip clip = settings.GetRandomClip();
            if (clip != null)
            {
                return clip.length;
            }
        }

        return 0f;
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

        if (repeatingSounds.TryGetValue(sound, out Coroutine coroutine))
        {
            AudioSourcePool.instance.StopCoroutine(coroutine);
            repeatingSounds.Remove(sound);

            Debug.Log($"Stopping repeating {sound}");
        }
    }

    private static IEnumerator ReturnToPoolAfterPlaying(AudioSource audioSource, Sound sound)
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        if (activeSounds.ContainsKey(sound) && activeSounds[sound] == audioSource)
        {
            activeSounds.Remove(sound);
        }

        AudioSourcePool.instance.ReturnAudioSource(audioSource);
    }
}