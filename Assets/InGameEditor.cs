using UnityEngine;

using UnityEngine;

public class InGameEditor : MonoBehaviour
{
    private Vector2 scrollPosition;

    private void OnGUI()
    {
        // Define background color
        GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
        backgroundStyle.normal.background = MakeTex(600, 800, new Color(0f, 0f, 0f, 0.8f)); // semi-transparent black background

        // Set up the background area
        GUILayout.BeginArea(new Rect(10, 10, 620, 820), backgroundStyle);

        // Scroll view
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(600), GUILayout.Height(800));

        GUILayout.Label("In-Game Sound Settings", GUI.skin.label, GUILayout.Height(30));

        foreach (var soundSetting in GameAssets.instance.soundSettings)
        {
            GUILayout.Label("Sound: " + soundSetting.sound, GUI.skin.label, GUILayout.Height(20));

            GUILayout.Label("Volume", GUI.skin.label);
            soundSetting.volume = GUILayout.HorizontalSlider(soundSetting.volume, 0f, 1f, GUILayout.Width(200));
            GUILayout.Label(soundSetting.volume.ToString("F2"), GUI.skin.label);

            GUILayout.Label("Pitch", GUI.skin.label);
            soundSetting.pitch = GUILayout.HorizontalSlider(soundSetting.pitch, 0.1f, 3f, GUILayout.Width(200));
            GUILayout.Label(soundSetting.pitch.ToString("F2"), GUI.skin.label);

            GUILayout.Label("Spatial Blend", GUI.skin.label);
            soundSetting.spatialBlend = GUILayout.HorizontalSlider(soundSetting.spatialBlend, 0f, 1f, GUILayout.Width(200));
            GUILayout.Label(soundSetting.spatialBlend.ToString("F2"), GUI.skin.label);

            GUILayout.Label("Doppler Level", GUI.skin.label);
            soundSetting.dopplerLevel = GUILayout.HorizontalSlider(soundSetting.dopplerLevel, 0f, 5f, GUILayout.Width(200));
            GUILayout.Label(soundSetting.dopplerLevel.ToString("F2"), GUI.skin.label);

            GUILayout.Label("Reverb Zone Mix", GUI.skin.label);
            soundSetting.reverbZoneMix = GUILayout.HorizontalSlider(soundSetting.reverbZoneMix, 0f, 1.1f, GUILayout.Width(200));
            GUILayout.Label(soundSetting.reverbZoneMix.ToString("F2"), GUI.skin.label);

            GUILayout.Label("Min Distance", GUI.skin.label);
            soundSetting.minDistance = GUILayout.HorizontalSlider(soundSetting.minDistance, 0f, 50f, GUILayout.Width(200));
            GUILayout.Label(soundSetting.minDistance.ToString("F2"), GUI.skin.label);

            GUILayout.Label("Max Distance", GUI.skin.label);
            soundSetting.maxDistance = GUILayout.HorizontalSlider(soundSetting.maxDistance, 50f, 500f, GUILayout.Width(200));
            GUILayout.Label(soundSetting.maxDistance.ToString("F2"), GUI.skin.label);

            if (GUILayout.Button("Apply Changes", GUILayout.Height(20)))
            {
                GameAssets.instance.NotifySoundSettingsChanged(soundSetting.sound);
            }

            GUILayout.Space(20); // Add some space between settings
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    // Helper method to create a texture
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}

