using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueArc
    {
        public dialogueSet[] dialogueSets;
        public TextAsset recurringDialogue;
        public Vector2 finalWarpPoint;
        public int finalAreaNumber;
        
    }

    [System.Serializable]
    public struct dialogueSet
    {
        public TextAsset dialogue;
        public Vector2 warpPoint;
        public int areaNumber;
    }

    public DialogueArc[] dialogueArcs;
    public int timesSpokenTo = 0;
    [HideInInspector]
    public int arcLastSpokenTo = 0;

    public GameObject speechBubbleRefPoint;

    public TextAsset GetCurrentText(int arc)
    {

        UpdateNPCArc(arc);

        //creates a holder for the current dialogue set
        List<TextAsset> tempDialogueHolder = new List<TextAsset>();

        //checks if the current arc is greater than the number of dialogue arcs available
        if (arc >= dialogueArcs.Length)
        {
            for(int i = 0; i < dialogueArcs[dialogueArcs.Length - 1].dialogueSets.Length; i++)
            {
                tempDialogueHolder.Add(dialogueArcs[dialogueArcs.Length - 1].dialogueSets[i].dialogue);
            }

            TextAsset[] finalDialogueSet = tempDialogueHolder.ToArray();

            //checks if the player has spoken to the npc enough times to trigger the recurring dialogue
            if (timesSpokenTo >= finalDialogueSet.Length || finalDialogueSet.Length == 0)
                return (dialogueArcs[dialogueArcs.Length - 1].recurringDialogue);

            //otherwise, uses dialogue from the dialogue set
            else
                return (tempDialogueHolder[timesSpokenTo]);
        }
        //otherwise, chooses dialogue from current arc
        else
        {
            for (int i = 0; i < dialogueArcs[arc].dialogueSets.Length; i++)
            {
                tempDialogueHolder.Add(dialogueArcs[arc].dialogueSets[i].dialogue);
            }

            TextAsset[] currentDialogueSet = tempDialogueHolder.ToArray();

            //checks if the player has spoken to the npc enough times to trigger the recurring dialogue
            if (timesSpokenTo >= currentDialogueSet.Length || currentDialogueSet.Length == 0)
                return (dialogueArcs[arc].recurringDialogue);

            //otherwise, uses dialogue from the dialogue set
            else
                return (currentDialogueSet[timesSpokenTo]);
        }
    }

    //mainly used to reset the number of times the player has spoken to the npc
    void UpdateNPCArc(int currentArc)
    {
        //used for debug purposes
        int localArcRef = arcLastSpokenTo;

        //checks if the npc dialogue is not synced up with the current arc
        if (arcLastSpokenTo != currentArc)
        {
            //updates dialogue to current arc and resets the number of times the player has talked with the npc
            arcLastSpokenTo = currentArc;

            //resets times spoken to this arc as long as the current arc doesn't overextend the amount of dialogue arcs available (statement used after is for debugging)
            if(currentArc < dialogueArcs.Length || currentArc - localArcRef > 1)
                timesSpokenTo = 0;
        }
    }

}