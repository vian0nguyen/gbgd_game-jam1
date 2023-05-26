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

    //automatically fills in dialogue of all npc's based on quest
    public void FillNPCDialogue(QuestlineScriptableObj quest)
    {
        foreach (NPCScript character in npcs)
        {

            bool isInScene = false;

            foreach(QuestlineScriptableObj.character questCharacter in quest.characters)
            {
                if (questCharacter.NPCName.ToLower() == character.name.ToLower())
                {
                    character.dialogueArcs = questCharacter.dialogueArcs;
                    isInScene = true;
                    break;
                }
            }

            //only prints an error if the npc isn't found
            if(isInScene == false)
                Debug.LogError("NPC " + character.name + " in questline not found in scene!");
        }
    }

    //updates the npc list in all quests if an npc is added or removed from references
    public void UpdateNPCList()
    {
        foreach(QuestlineScriptableObj quest in quests)
        {
            if (npcs.Length != quest.characters.Count)
            {
                //checks if an npc was removed from the main list
                if (quest.characters.Count > npcs.Length)
                {
                    if (npcs.Length != 0)
                    {
                        foreach (QuestlineScriptableObj.character character in quest.characters)
                        {

                            foreach (NPCScript npc in npcs)
                            {
                                //if there is a matching npc, the skips to the next iteration
                                if (character.NPCName.ToLower() == npc.name.ToLower())
                                {
                                    continue;
                                }

                                quest.characters.Remove(character);

                            }

                        }
                    }
                    else
                    {
                        quest.characters.Clear();
                    }
                }

                //checks if an npc was added to the main list
                else if (quest.characters.Count < npcs.Length)
                {
                    if (quest.characters.Count != 0) {

                        foreach (QuestlineScriptableObj.character character in quest.characters)
                        {

                            foreach (NPCScript npc in npcs)
                            {
                                //if there is a matching npc, the skips to the next iteration
                                if (character.NPCName.ToLower() == npc.name.ToLower())
                                {
                                    continue;
                                }

                                QuestlineScriptableObj.character newCharacter = new QuestlineScriptableObj.character();
                                newCharacter.NPCName = npc.name;

                                quest.characters.Add(newCharacter);

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

                                quest.characters.Add(newCharacter);
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
        foreach (QuestlineScriptableObj quest in quests)
        {
            for (int i = 0; i < quest.characters.Count; i++)
            {
                if (quest.characters[i].NPCName != npcs[i].name)
                {
                    QuestlineScriptableObj.character newCharacter = new QuestlineScriptableObj.character();

                    newCharacter.NPCName = npcs[i].name;
                    newCharacter.dialogueArcs = quest.characters[i].dialogueArcs;

                    quest.characters[i] = newCharacter;
                }
            }
        }
    }

}
