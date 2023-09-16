using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WarpPointPlugInTool : EditorWindow
{
    public QuestlineScriptableObj questObject;
    public areaManager areaManager;
    public string characterName;
    public int arcForWarpPoints;

    //use container and get children
    public Transform[] warpPoints;
    public Transform warpPointsContainer;
    public Transform finalWarpPoint;

    [MenuItem("Tools/Plug In Warp Points")]
    static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(WarpPointPlugInTool));
        window.Show();
    }

    void OnGUI()
    {
        questObject = EditorGUILayout.ObjectField("Quest Scriptable Object: ", questObject, typeof(QuestlineScriptableObj), true) as QuestlineScriptableObj;
        characterName = EditorGUILayout.TextField("NPC Name: ", characterName);
        arcForWarpPoints = EditorGUILayout.IntField("Arc Number: ", arcForWarpPoints);

        //Found here: https://stackoverflow.com/questions/47753367/how-to-display-modify-array-in-the-editor-window answer by Victor
        ScriptableObject target = this; //makes a temporary scriptable object to reference
        SerializedObject so = new SerializedObject(target); //makes a serializable object from the scriptable object (so that we can access the properties and display them)
        SerializedProperty serializedWarpPoints = so.FindProperty("warpPoints"); //finds property to display because we can't display it normally

        EditorGUILayout.PropertyField(serializedWarpPoints, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button("Create"))
        {
            FillWarpPoints();
        }
            
    }

    void FillWarpPoints()
    {
        List<Vector2> warpPointVectors = new List<Vector2>();

        foreach (Transform warpPoint in warpPoints)
        {
            warpPointVectors.Add(warpPoint.position);
        }

        foreach (QuestlineScriptableObj.character npc in questObject.characters)
        {
            if (npc.NPCName.ToUpper() == characterName.ToUpper())
            {
                //npc.dialogueArcs[arcForWarpPoints].warpPoints = warpPointVectors.ToArray();

                break;
            }
        }
    }

}