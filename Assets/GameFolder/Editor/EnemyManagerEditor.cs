using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor
{
    SerializedProperty wavesProperty;

    private void OnEnable()
    {
        wavesProperty = serializedObject.FindProperty("waves");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Wave Configurations", EditorStyles.boldLabel);

        for (int i = 0; i < wavesProperty.arraySize; i++)
        {
            SerializedProperty waveProperty = wavesProperty.GetArrayElementAtIndex(i);

            if (waveProperty.objectReferenceValue != null)
            {
                EditorGUILayout.PropertyField(waveProperty, new GUIContent($"Wave {i + 1}"), true);

                Editor nestedEditor = CreateEditor(waveProperty.objectReferenceValue);
                EditorGUI.indentLevel++;
                nestedEditor.OnInspectorGUI();
                EditorGUI.indentLevel--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
