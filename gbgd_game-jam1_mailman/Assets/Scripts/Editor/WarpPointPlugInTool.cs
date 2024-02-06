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

    [SerializeField] public Transform[] warpPoints;
    public Transform recurringWarpPoint;

    [MenuItem("Tools/Plug In Warp Points")]
    static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(WarpPointPlugInTool));
        window.Show();
    }

    private void OnEnable()
    {

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
                //warpPoints = new List<Transform>(new Transform[currentCharacter.dialogue.dialogueSets.Length]);

                //sets up a temporary 
                int currentSize = currentCharacter.dialogue.dialogueSets.Length;

                if(warpPoints.Length != currentSize)
                {
                    ResizeWarpPointArray(currentSize);
                }
                
                for (int i = 0; i < warpPoints.Length; i++) {
                    Transform point = warpPoints[i];
                    warpPoints[i] = EditorGUILayout.ObjectField("Warp Point " + i, point, typeof(Transform), true) as Transform;
                }

            }

            //prints an error if an invalid arc number is input
            else
            {
                Debug.LogError("Please enter a valid arc number");
            }
        }
        
        //prints an error if a name that isn't in the quest is input
        else
        {
            Debug.LogError("Please add a valid name from the quest");
        }

        recurringWarpPoint = EditorGUILayout.ObjectField("Final Warp Point", recurringWarpPoint, typeof(Transform), true) as Transform;

        so.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button("Set Warp Points from Transforms"))
        {
            FillWarpPoints();
        }
            
    }

    //fills in warp points for this specific character in this specific arc
    void FillWarpPoints()
    {

        //checks if there is a character and a quest
        if (characterName != null && questObject != null && characterName !="-1")
        {
            //runs through all warp points in the tool
            for (int i = 0; i < warpPoints.Length; i++)
            {
                //sets warp points to character in QUEST
                GetCharacter(characterName).dialogue.dialogueSets[i].warpPoint = warpPoints[i].position;
            }
            //sets recurring warp point position for this arc
            GetCharacter(characterName).dialogue.finalWarpPoint = recurringWarpPoint.position;

        }
        //checks if there isn't a valid character name or arc number
        else
        {
            Debug.LogError("Cannot set warp points because either the entered name is invalid or there is no valid questObject");
        }
        
    }

    #region Character Name Functions
    //checks if the name typed in exists <- Only works if the quest has actual names and arcs set up already
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
    #endregion

    //resizes the warp point array if the amount of warp points changed
    void ResizeWarpPointArray(int sizeRef)
    {
        warpPoints = new Transform[sizeRef];
    }

}