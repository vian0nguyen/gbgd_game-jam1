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
    public Transform warpPointContainer;
    public Transform warpPointsContainer;

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

        if (GUILayout.Button("Set Warp Points from Transforms"))
        {
            FillWarpPoints();
        }
            
    }

    void FillWarpPoints()
    {
        List<Vector2> warpPointVectors = new List<Vector2>();

        //initial check in case no warp points were put into the warp container
        if(warpPointContainer.childCount != 0)
        {
            Debug.LogError("Please make sure that the warp point container has child objects that serve as the markers for where you want the character to be in later dialogue!");
            return;
        }

        //runs through all warp points listed by the USER
        foreach (Transform warpPoint in warpPointContainer)
        {
            warpPointVectors.Add(warpPoint.position);
        }

        //runs through every character in the arc in the QUEST
        foreach (QuestlineScriptableObj.character npc in questObject.dialogueArcs[arcForWarpPoints].charactersSpeaking)
        {
            //checks if the names match
            if (npc.NPCName.ToUpper() == characterName.ToUpper())
            {

                //runs through all warp points in the tool
                for (int i = 0; i < warpPointVectors.Count - 1; i++)
                {
                    //sets warp points to character in QUEST
                    npc.dialogue.dialogueSets[i].warpPoint = warpPointVectors[i];
                }

                break;
            }
        }
    }

}