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

    [SerializeField] public List<Transform> warpPoints;

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

        if (ValidateName(characterName))
        {
            QuestlineScriptableObj.character currentCharacter = GetCharacter(characterName);

            if(arcForWarpPoints >= 0 && arcForWarpPoints < questObject.dialogueArcs.Length)
            {
                //if you don't want the character to say anything new this arc, make their dialogue for this arc all recurring

                warpPoints = new List<Transform>(new Transform[currentCharacter.dialogue.dialogueSets.Length]);
                Debug.Log("cool");
                
                for (int i = 0; i < warpPoints.Count; i++) {
                    Transform point = warpPoints[i];
                    EditorGUILayout.ObjectField("Warp Point " + i, point, typeof(Transform), true);
                }
            }
        }

        so.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button("Set Warp Points from Transforms"))
        {
            FillWarpPoints();
        }
            
    }

    void FillWarpPoints()
    {
        List<Vector2> warpPointVectors = new List<Vector2>();

        //runs through all warp points listed by the USER
        /*foreach (Transform warpPoint in warpPointContainer)
        {
            warpPointVectors.Add(warpPoint.position);
            warpPointScript wpc = warpPoint.GetComponent<warpPointScript>();

            if (wpc.repeats)
            {
                for(int i = 0; i < wpc.numberOfTimesRepeated - 1; i++)
                {
                    warpPointVectors.Add(warpPoint.position);
                }
            }

        }*/

        //checks if there is a character and a quest
        if ((characterName != null ) && questObject != null)
        {

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
        //new prefab: transforms that have scripts that say whether or not to repeat a warp point, also does math for if the number of repeats don't add up, and has parent transforms
    }

    //checks if the name typed in exists
    bool ValidateName(string npcName)
    {
        if (npcName != null & questObject != null)
        {

            foreach (QuestlineScriptableObj.character npc in questObject.dialogueArcs[arcForWarpPoints].charactersSpeaking)
            {
                //checks if the names match
                if (npc.NPCName.ToUpper() == characterName.ToUpper())
                {

                    return true;

                }
            }

        }
        return false;
    }

    QuestlineScriptableObj.character GetCharacter(string name)
    {
        //runs through every character in the arc in the QUEST
        foreach (QuestlineScriptableObj.character npc in questObject.dialogueArcs[arcForWarpPoints].charactersSpeaking)
        {
            //checks if the names match
            if (npc.NPCName.ToUpper() == name.ToUpper())
            {
                return npc;
            }
        }

        QuestlineScriptableObj.character blankCharacter = new QuestlineScriptableObj.character();
        blankCharacter.NPCName = "-1";

        return blankCharacter;
    }

}