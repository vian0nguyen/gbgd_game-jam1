using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class QuestManager : MonoBehaviour
{
    public QuestlineScriptableObj[] quests;
    public QuestlineScriptableObj currentQuest;

    public NPCScript[] npcs;

    private void Start()
    {
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

                    //if the name in the scene doesn't match the arc, then returns an error
                    else
                    {
                        Debug.LogError("Could not find character from quest in the game scene!");
                    }
                }

            }
        }
    }

    //updates the npc list in all quests if an npc is added or removed from references
    public void UpdateNPCList()
    {
        //runs through each quest
        foreach(QuestlineScriptableObj quest in quests)
        {
            //runs through every arc in the quest
            foreach(QuestlineScriptableObj.arc arc in quest.dialogueArcs) {

                //checks if the list of npcs talking is the same length in the scene as the quest
                if (npcs.Length != arc.charactersSpeaking.Count)
                {
                    //checks if an npc was removed from the main list
                    if (arc.charactersSpeaking.Count > npcs.Length)
                    {
                        //checks if the list actually has any npcs
                        if (npcs.Length != 0)
                        {
                            //runs through every character in the arc in the QUEST
                            foreach (QuestlineScriptableObj.character character in arc.charactersSpeaking)
                            {

                                foreach (NPCScript npc in npcs)
                                {
                                    //if there is a matching npc, the skips to the next iteration
                                    if (character.NPCName.ToLower() == npc.name.ToLower())
                                    {
                                        continue;
                                    }

                                    arc.charactersSpeaking.Remove(character);

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
                        if (arc.charactersSpeaking.Count != 0)
                        {
                            //runs through every character in the arc in the QUEST
                            foreach (QuestlineScriptableObj.character character in arc.charactersSpeaking)
                            {
                                //runs through all the npcs in the SCENE
                                foreach (NPCScript npc in npcs)
                                {
                                    //if there is a matching npc, the skips to the next iteration
                                    if (character.NPCName.ToLower() == npc.name.ToLower())
                                    {
                                        continue;
                                    }

                                    QuestlineScriptableObj.character newCharacter = new QuestlineScriptableObj.character();
                                    newCharacter.NPCName = npc.name;

                                    arc.charactersSpeaking.Add(newCharacter);

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
        //runs through all quests
        foreach (QuestlineScriptableObj quest in quests)
        {
            //runs through all the arcs in each quest
            foreach (QuestlineScriptableObj.arc arc in quest.dialogueArcs)
            {
                //runs through all npcs in each quest
                for (int i = 0; i < arc.charactersSpeaking.Count; i++)
                {
                    //checks if the name in the scene matches the one in the quest
                    if (arc.charactersSpeaking[i].NPCName != npcs[i].name)
                    {
                        QuestlineScriptableObj.character newCharacter = new QuestlineScriptableObj.character();

                        newCharacter.NPCName = npcs[i].name;
                        newCharacter.dialogue = arc.charactersSpeaking[i].dialogue;

                        arc.charactersSpeaking[i] = newCharacter;
                    }
                }

            }
        }
    }

    //checks to see if all the values in the npc array are valid
    bool ValidateSceneNPCValues()
    {
        foreach(NPCScript npc in npcs)
        {
            if (npc == null)
                return false;
        }

        return true;
    }

    //updates when values in the INSPECTOR change
    private void OnValidate()
    {
        //checks if all the values in the npcs array are valid, then updates the list
        if (ValidateSceneNPCValues())
        {
            
        }
    }

    #endregion

    public void MoveAllNPCs()
    {
        for (int i = 0; i < npcs.Length; i++)
        {

        }
    }

}
