using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteAlways]
public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;
    [SerializeField]
    private areaManager am;

    public QuestlineScriptableObj[] quests;
    public QuestlineScriptableObj currentQuest;

    public NPCScript[] npcs;

    private QuestlineScriptableObj.character lastDeletedCharacter;
    private List<NPCScript> NPCsToMove;

    private void Start()
    {
        NPCsToMove = new List<NPCScript>();

        //checks if there is a current quest to start with
        if (currentQuest != null)
        {
            FillNPCDialogue(currentQuest);
        }
    }

    //sets current quest based on what item the player picked
    public void SetCurrentQuest(string questName)
    {
        //checks through all the quests
        foreach(QuestlineScriptableObj quest in quests)
        {
            //finds matching quest and sets it to the current one
            if (quest.name.ToLower() == questName.ToLower()){
                currentQuest = quest;
                FillNPCDialogue(currentQuest);
                return;
            }
        }

        //otherwise, prints an error
        Debug.LogError("Could not find quest!  Please use a valid quest name in the tag");
    }

    //Finds npc based on name
    public NPCScript FindNPC (string name)
    {
        foreach (NPCScript npc in npcs)
        {
            if (npc.gameObject.name.ToLower() == name.ToLower())
            {
                return npc;
            }
        } 

        return null;
    }

    #region NPC Dialogue List Fill In
    //automatically fills in dialogue of all npc's based on quest
    public void FillNPCDialogue(QuestlineScriptableObj quest)
    {
        //goes through all npcs listed from the scene
        foreach (NPCScript character in npcs)
        {
            //sets number of arcs for each character
            character.dialogueArcs = new NPCScript.DialogueArc[quest.dialogueArcs.Length];

            //goes through all the dialogue arcs in the quest
            for(int k = 0; k < quest.dialogueArcs.Length; k++)
            {

                //goes through all the characters in the arc
                for(int i=0; i < quest.dialogueArcs[k].charactersSpeaking.Count; i++)
                {
                    //checks if the character in the scene matches one in the arc
                    if(quest.dialogueArcs[k].charactersSpeaking[i].NPCName.ToUpper() == character.name.ToUpper())
                    {
                        //sets npc's dialogue arc to whatever was in the scriptable object
                        character.dialogueArcs[k] = quest.dialogueArcs[k].charactersSpeaking[i].dialogue;
                        break;
                    }
                }

            }
        }
    }

    //updates the npc list in all quests if an npc is added or removed from references
    public void UpdateNPCList()
    {
        //creates a temporary list to compare names
        List<string> nameHolder = new List<string>();
        foreach (NPCScript npc in npcs)
        {
            nameHolder.Add(npc.name);
        }

        //runs through each quest
        foreach (QuestlineScriptableObj quest in quests)
        {
            //runs through every arc in the quest
            foreach (QuestlineScriptableObj.arc arc in quest.dialogueArcs) {

                //checks if the list of npcs talking is the same length in the scene as the quest
                if (npcs.Length != arc.charactersSpeaking.Count)
                {
                    //checks if an npc was removed from the main list
                    if (arc.charactersSpeaking.Count > npcs.Length)
                    {
                        //checks if the list actually has any npcs
                        if (npcs.Length > 0)
                        {
                            //runs through all npcs
                            for (int i = 0; i < arc.charactersSpeaking.Count; i++)
                            {
                                //checks if the name of the current npc matches 
                                if (!nameHolder.Contains(arc.charactersSpeaking[i].NPCName))
                                {
                                    arc.charactersSpeaking.Remove(arc.charactersSpeaking[i]);
                                    print("Character removed");
                                }

                                //removes any duplicates
                                for (int k = i - 1; k >= 0; k--)
                                {
                                    if(arc.charactersSpeaking[k].NPCName.ToUpper() == arc.charactersSpeaking[i].NPCName.ToUpper())
                                    {
                                        arc.charactersSpeaking.Remove(arc.charactersSpeaking[k]);
                                        print("Character removed");
                                    }
                                }
                            }

                        }
                        
                        //if there are no npcs in the list, it clears
                        else
                        {
                            arc.charactersSpeaking.Clear();
                        }

                    }

                    //checks if an npc was added to the main list
                    else if (arc.charactersSpeaking.Count < npcs.Length)
                    {

                        //checks if there are actually any npcs in the list
                        if (arc.charactersSpeaking.Count > 0)
                        {

                            //creates a temporary list to compare names
                            nameHolder = new List<string>();
                            foreach (QuestlineScriptableObj.character character in arc.charactersSpeaking)
                            {
                                nameHolder.Add(character.NPCName);
                            }

                            //runs through all npcs
                            for (int i = 0; i < npcs.Length; i++)
                            {
                                //checks if the name of the current npc matches in the QUEST
                                if (!nameHolder.Contains(npcs[i].name))
                                {
                                    QuestlineScriptableObj.character newCharacter = new QuestlineScriptableObj.character();
                                    newCharacter.NPCName = npcs[i].name;
                                    arc.charactersSpeaking.Add(newCharacter);
                                    print("Character added");
                                }
                            }

                            //if the number of npcs in the scene is STILL more than the npcs in the quest, adds a blank character slot (for duplicates)
                            if(npcs.Length > arc.charactersSpeaking.Count)
                            {
                                for (int h = 0; h < npcs.Length - arc.charactersSpeaking.Count; h++)
                                {
                                    QuestlineScriptableObj.character newBlankCharacter = new QuestlineScriptableObj.character();
                                    newBlankCharacter.NPCName = "Blank NPC";
                                    arc.charactersSpeaking.Add(newBlankCharacter);
                                    print("Blank Character Added");
                                }
                            }
                        }

                        //checks if the character list is 0
                        else
                        {
                            foreach (NPCScript npc in npcs)
                            {
                                //checks if there is an npc to fill in
                                if (npc)
                                {
                                    QuestlineScriptableObj.character newCharacter = new QuestlineScriptableObj.character();
                                    newCharacter.NPCName = npc.name;

                                    arc.charactersSpeaking.Add(newCharacter);
                                }
                            }

                        }
                    }
                }
            }
        }
    }

    //updates name of npcs without having to set the list count to zero and re plug everything in
    public void UpdateNPCNames()
    {
        npcs = npcs.OrderByDescending(npcName => npcName.name).Reverse().ToArray();

        //runs through all quests
        foreach (QuestlineScriptableObj quest in quests)
        {
            //runs through all the arcs in each quest
            foreach (QuestlineScriptableObj.arc arc in quest.dialogueArcs)
            {
                //creates a temporary list and reorders the actual list
                List<QuestlineScriptableObj.character> reorderHolder = new List<QuestlineScriptableObj.character>();
                foreach (QuestlineScriptableObj.character oldnpc in arc.charactersSpeaking)
                {
                    reorderHolder.Add(oldnpc);
                }
                reorderHolder.OrderByDescending(reorderChar => reorderChar.NPCName).Reverse().ToList();
                arc.charactersSpeaking.Clear();
                foreach (QuestlineScriptableObj.character reorderedCharacter in reorderHolder)
                {
                    arc.charactersSpeaking.Add(reorderedCharacter);
                }

                //creates a temporary list
                List<QuestlineScriptableObj.character> tempCharacters = new List<QuestlineScriptableObj.character>();

                //creates a temporary list to compare names
                List<string> nameHolder = new List<string>();
                foreach (NPCScript npc in npcs)
                {
                    nameHolder.Add(npc.name);
                }

                //creates a temporary list to compare names
                List<string> questNameHolder = new List<string>();
                foreach (QuestlineScriptableObj.character questCharacter in arc.charactersSpeaking)
                {
                    questNameHolder.Add(questCharacter.NPCName);
                }

                //runs through all the npcs in the scene
                for (int i = 0; i < arc.charactersSpeaking.Count; i++)
                {
                    //checks if the name is NOT in the list of npcs
                    if(!nameHolder.Contains(arc.charactersSpeaking[i].NPCName))
                    {
                        //creates a new character
                        QuestlineScriptableObj.character replaceNPC = new QuestlineScriptableObj.character();
                        foreach(NPCScript sceneNPC in npcs)
                        {
                            //checks if the npc name is NOT in the quest
                            if (!questNameHolder.Contains(sceneNPC.name))
                            {
                                replaceNPC.NPCName = sceneNPC.name;
                                break;
                            }
                        }


                        arc.charactersSpeaking[i] = replaceNPC;
                        print("Character replaced");
                    }
                    tempCharacters.Add(arc.charactersSpeaking[i]);
                }

                //clears out list and readds everything
                tempCharacters = tempCharacters.OrderByDescending(tempChar => tempChar.NPCName).Reverse().ToList();
                arc.charactersSpeaking.Clear();
                foreach(QuestlineScriptableObj.character tempCharacter in tempCharacters)
                {
                    arc.charactersSpeaking.Add(tempCharacter);
                }
            }
        }
    }

    #endregion

    #region NPC Functions

    //moves all npcs (used for end of an arc)
    public void MoveAllNPCsOnReset()
    {
        foreach(NPCScript npc in npcs)
        {
            MoveNPC(npc);
        }
    }

    //moves all npcs that are to move at the end of a dialogue
    public void MoveNPCsPostDialogue()
    {
        foreach (NPCScript npc in NPCsToMove)
        {
            MoveNPC(npc);
        }
    }

    //moves npc to destination
    public void MoveNPC(NPCScript npc)
    {
        NPCScript.dialogueSet chosenSet;

        //checks if there is NO more dialogue after this arc
        if (gm.arc >= npc.dialogueArcs.Length)
        {
            // check if times spoken to is greater than size of dialogue sets for recurring

            //checks if the number of times spoken to is LESS than the amount of dialogue available in this arc's dialogue set
            if(npc.timesSpokenTo <= npc.dialogueArcs[npc.dialogueArcs.Length - 1].dialogueSets.Length)
            {
                chosenSet = npc.dialogueArcs[npc.dialogueArcs.Length - 1].dialogueSets[npc.timesSpokenTo - 1];
            }

            //checks if the number of times spoken to is GREATER than the amount of dialogue available in this arc's dialogue set
            else
            {
                chosenSet = new NPCScript.dialogueSet();
                    
                chosenSet.areaNumber = npc.dialogueArcs[npc.dialogueArcs.Length - 1].finalAreaNumber;
                chosenSet.warpPoint = npc.dialogueArcs[npc.dialogueArcs.Length - 1].finalWarpPoint;
            }

        }
        //checks if there IS dialogue after this arc
        else
        {
            
            //checks if the number of times spoken to is LESS than the amount of dialogue available in this arc's dialogue set
            if (npc.timesSpokenTo <= npc.dialogueArcs[gm.arc].dialogueSets.Length)
            {
                chosenSet = npc.dialogueArcs[gm.arc].dialogueSets[npc.timesSpokenTo - 1];
            }

            //checks if the number of times spoken to is GREATER than the amount of dialogue available in this arc's dialogue set
            else
            {
                chosenSet = new NPCScript.dialogueSet();

                chosenSet.areaNumber = npc.dialogueArcs[gm.arc].finalAreaNumber;
                chosenSet.warpPoint = npc.dialogueArcs[gm.arc].finalWarpPoint;
            }
        }

        //checks if the area number is NOT negative or the area number given is within the bounds of the number of areas available
        if (chosenSet.areaNumber >= 0 || chosenSet.areaNumber < am.areas.Length)
        {
            //changes parent object of NPC and moves it
            npc.transform.parent = am.areas[chosenSet.areaNumber].transform;
            npc.transform.position = chosenSet.warpPoint;
        }

        //prints a warning if the npc is set to be moved but isn't due to an invalid area number
        else
        {
            Debug.LogWarning("NPC not moved because it has an invalid area number to move to!");
        }
    }

    //adds npc to list of npc's to move after the dialogue is over
    public void AddNPCToMove(string npcName) {
        
        //checks if there is an npc available
        if (FindNPC(npcName) != null)
        {
            NPCsToMove.Add(FindNPC(npcName));
        }
    }
    #endregion
}
