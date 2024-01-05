using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(warpPointScript))]
[CanEditMultipleObjects]
public class WarpPointEditorScript : Editor
{
    SerializedProperty numberOfTimesRepeated;
    SerializedProperty repeats;
    SerializedProperty parentTransform;

    private void OnEnable()
    {
        numberOfTimesRepeated = serializedObject.FindProperty("numberOfTimesRepeated");
        repeats = serializedObject.FindProperty("repeats");
        parentTransform = serializedObject.FindProperty("parentTransform");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //sets up visible ui
        EditorGUILayout.PropertyField(parentTransform);
        EditorGUILayout.PropertyField(repeats);

        //only shows this ui if the warp point repeats
        if (repeats.boolValue)
        {
            EditorGUILayout.PropertyField(numberOfTimesRepeated); 
        }

        serializedObject.ApplyModifiedProperties();
    }
}
