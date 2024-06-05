using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class GameAssets : MonoBehaviour
{

    private static GameAssets _instance;
    public static GameAssets instance
    {
        get
        {
            if (_instance == null) _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _instance;
        }
    }

    [Header("Sound Settings")]
    public SoundSettings[] soundSettings;
    
    [System.Serializable]
    public class SoundSettings
    {
        public SoundManager.Sound sound;
        public AudioClip[] audioClips;
        public AudioMixerGroup mixerGroup;
        public bool playOnAwake;
        public bool loop;
        public bool mute;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        [Range(0f, 1f)] public float spatialBlend = 1f;
        [Range(0f,5f)] public float dopplerLevel = 1f;
        [Range(0f,1.1f)] public float reverbZoneMix = 1f;
        public AudioRolloffMode volumeRolloff = AudioRolloffMode.Logarithmic;
        public float minDistance = 1f;
        public float maxDistance = 500f;
        

        public AudioClip GetRandomClip()
        {
            if (audioClips.Length == 0)
                return null;
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}
