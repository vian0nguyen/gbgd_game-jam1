using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Writer_Prototype_1 : Dialogue_Writer
{

	public gameManager_Prototype_1 gmp1;

    public override void Awake()
    {
		base.Awake();
		
		//checks if the gameManager is the one specifically for prototype 1
		if (gm is gameManager_Prototype_1)
			gmp1 = gm as gameManager_Prototype_1;
	}

    public override void CheckInteractionInput()
    {
		switch (gmp1.currentState)
		{
			case GameManager.GameState.NotTalking:
				gm.currentState = GameManager.GameState.Talking;
				GetNPCTextAsset();
				ShowUI();
				StartStory();
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

	public override void GetNPCTextAsset()
    {
		//gets dialogue catalogue from NPC
		NPCScript npcInfo = gmp1.currentNPC.GetComponent<NPCScript>();

		//Sets dialogue based on what arc the game is currently on
		inkJSONAsset = npcInfo.GetCurrentText(gmp1.arc);

		//increments number of times the player has spoken to this npc
		npcInfo.timesSpokenTo++;
    }
	
}
