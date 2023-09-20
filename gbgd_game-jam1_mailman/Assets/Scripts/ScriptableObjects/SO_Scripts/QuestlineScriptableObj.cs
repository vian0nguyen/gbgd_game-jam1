using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the purpose of this is to set up linear questlines for all the npcs in the scene
[CreateAssetMenu(fileName = "Questline", menuName = "ScriptableObjects/Questline")]
public class QuestlineScriptableObj : ScriptableObject
{
    [System.Serializable]
    public struct character
    {
        public string NPCName;

        public NPCScript.DialogueArc dialogue;
    }

    //public List<character> characters;
    
    [System.Serializable]
    public struct arc
    {
        public List<character> charactersSpeaking;
    }

    public arc[] dialogueArcs;
}
