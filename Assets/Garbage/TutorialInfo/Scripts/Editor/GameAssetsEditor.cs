using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameAssets))]
public class GameAssetsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GameAssets gameAssets = (GameAssets)target;
        
        EditorGUILayout.LabelField("Sounds", EditorStyles.whiteLargeLabel, GUILayout.ExpandHeight(true));

        SerializedProperty soundSettings = serializedObject.FindProperty("soundSettings");

        for (int i = 0; i < soundSettings.arraySize; i++)
        {
            int counter = i + 1;
            EditorGUILayout.LabelField("Sound " + counter, EditorStyles.boldLabel);
            
            SerializedProperty soundSetting = soundSettings.GetArrayElementAtIndex(i);


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("sound"), new GUIContent("Sound"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("mixerGroup"), new GUIContent("Mixer Group"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("loop"), new GUIContent("Loop"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("mute"), new GUIContent("Mute"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("volume"), new GUIContent("Volume"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("pitch"), new GUIContent("Pitch"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("spatialBlend"), new GUIContent("Spatial Blend"));
            
            EditorGUILayout.LabelField("3D Sound Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("dopplerLevel"), new GUIContent("Doppler Level"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("reverbZoneMix"), new GUIContent("Reverb Zone Mix"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("volumeRolloff"), new GUIContent("Volume Rolloff"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("minDistance"), new GUIContent("Min Distance"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("maxDistance"), new GUIContent("Max Distance"));

            SerializedProperty audioClips = soundSetting.FindPropertyRelative("audioClips");
            EditorGUILayout.PropertyField(audioClips, new GUIContent("Audio Clips"), true);

            if (GUILayout.Button("Remove Sound"))
            {
                soundSettings.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Sound"))
        {
            soundSettings.InsertArrayElementAtIndex(soundSettings.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    
}
