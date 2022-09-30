using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{

    public struct DialogueArc
    {
        public TextAsset[] dialogueSet;
        public TextAsset recurringDialogue;
    }

    public DialogueArc[] dialogueArcs;
    public int timesSpokenTo = 0;

}
