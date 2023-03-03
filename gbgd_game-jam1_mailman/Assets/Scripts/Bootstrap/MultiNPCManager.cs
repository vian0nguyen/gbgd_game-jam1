using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script exists because skipping the scroll stops all coroutines in the dialogue writer
public class MultiNPCManager : MonoBehaviour
{
    public GameManager gm;

	//Resets multiNPC sprites
	public void ResetMultiNPC()
	{

		NPCMultiScript npcMulti = null;

		//gets npc multi sprite if it exists
		if (gm.currentNPC is NPCMultiScript)
		{
			npcMulti = gm.currentNPC as NPCMultiScript;

			StartCoroutine(npcMulti.ResetSprites());
		}
	}

	//picks a sprite to highlight if the npc has multiple
	public void ChooseSpeakingSprite(string talkingSprite)
	{
		NPCMultiScript npcMulti = null;

		//gets npc multi sprite if it exists
		if (gm.currentNPC is NPCMultiScript)
		{
			npcMulti = gm.currentNPC as NPCMultiScript;

			//runs through all sprites in the npc to either darken or lighten them
			for (int i = 0; i < npcMulti.sprites.Length; i++)
			{
				if (npcMulti.sprites[i].gameObject.name != talkingSprite)
				{
					StartCoroutine(npcMulti.DarkenSprite(npcMulti.sprites[i]));
				}
				else
				{
					StartCoroutine(npcMulti.GrowSprite(npcMulti.sprites[i]));
				}
			}

		}
	}

}
