using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestManager))]
[CanEditMultipleObjects]

public class QuestUpdater_Editor : Editor
{
    QuestManager qm;

    void OnEnable()
    {
        //creates reference for the object being inspected (allows you to use functions in the class)
        qm = (QuestManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(20);

        //creates button for updating npc names
        if (GUILayout.Button("Update Names", GUILayout.Height(50))) {
            qm.UpdateNPCList();
            qm.UpdateNPCNames();
            AssetDatabase.SaveAssets();
        }

    }

}
