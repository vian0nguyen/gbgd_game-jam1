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
				case GameState.NotTalking:
					//checks if the npc has dialogue to begin with
					if (gmp3.currentNPC.GetComponent<NPCScript>().dialogueArcs.Length > 0)
					{
						gm.currentState = GameState.Talking;
						GetNPCTextAsset();
						ShowUI();
						StartStory();
					}
					break;

				case GameState.Talking:
					SkipScroll();
					break;

				case GameState.WaitingToAdvance:
					gm.currentState = GameState.Talking;
					RefreshView();
					//play sound here?
					break;
			}
		}
	}

}
