using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameAssets))]
public class GameAssetsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GameAssets gameAssets = (GameAssets)target;

        EditorGUILayout.LabelField("Sound Settings", EditorStyles.boldLabel);

        SerializedProperty soundSettings = serializedObject.FindProperty("soundSettings");

        for (int i = 0; i < soundSettings.arraySize; i++)
        {
            SerializedProperty soundSetting = soundSettings.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("sound"), new GUIContent("Sound"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("volume"), new GUIContent("Volume"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("pitch"), new GUIContent("Pitch"));
            EditorGUILayout.PropertyField(soundSetting.FindPropertyRelative("spatialBlend"), new GUIContent("Spatial Blend"));

            SerializedProperty audioClips = soundSetting.FindPropertyRelative("audioClips");
            EditorGUILayout.PropertyField(audioClips, new GUIContent("Audio Clips"), true);

            if (GUILayout.Button("Remove Sound Setting"))
            {
                soundSettings.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Sound Setting"))
        {
            soundSettings.InsertArrayElementAtIndex(soundSettings.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
