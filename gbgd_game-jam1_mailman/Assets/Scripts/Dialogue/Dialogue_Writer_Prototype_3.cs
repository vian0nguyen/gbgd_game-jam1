using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Writer_Prototype_3 : Dialogue_Writer
{

	public gameManager_Prototype_3 gmp3;

    public override void Awake()
    {
		base.Awake();
		
		//checks if the gameManager is the one specifically for prototype 3
		if (gm is gameManager_Prototype_3)
			gmp3 = gm as gameManager_Prototype_3;
	}

    public override void CheckInteractionInput()
    {
		//only does this check if there's an npc to talk to
		if (gmp3.currentNPC != null)
		{
			switch (gmp3.currentState)
			{
				case GameManager.GameState.NotTalking:
					//checks if the npc has dialogue to begin with
					if (gmp3.currentNPC.GetComponent<NPCScript>().dialogueArcs.Length > 0)
					{
						gm.currentState = GameManager.GameState.Talking;
						GetNPCTextAsset();
						ShowUI();
						StartStory();
					}
					break;

				case GameManager.GameState.Talking:
					SkipScroll();
					break;

				case GameManager.GameState.WaitingToAdvance:
					gm.currentState = GameManager.GameState.Talking;
					RefreshView();
					//play sound here?
					break;
			}
		}
	}

	public override void GetNPCTextAsset()
    {
		//gets dialogue catalogue from NPC
		NPCScript npcInfo = gmp3.currentNPC.GetComponent<NPCScript>();

		//Sets dialogue based on what arc the game is currently on
		inkJSONAsset = npcInfo.GetCurrentText(gmp3.arc);

		//increments number of times the player has spoken to this npc
		npcInfo.timesSpokenTo++;
    }

}
