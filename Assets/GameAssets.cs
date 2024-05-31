using UnityEngine;

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
        public SoundCategory category;
        public AudioClip[] audioClips;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        [Range(0f, 1f)] public float spatialBlend = 1f;

        public AudioClip GetRandomClip()
        {
            if (audioClips.Length == 0)
                return null;
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
    
    public enum SoundCategory
    {
        Music,
        SFX,
        UI,
        Voice,
    }
}
