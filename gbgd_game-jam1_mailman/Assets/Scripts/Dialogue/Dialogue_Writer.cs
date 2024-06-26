﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Dialogue_Writer : MonoBehaviour
{
	public static event Action<Story> OnCreateStory;

	public virtual void Awake()
	{
		//sets up variables to be reset upon the scene starting
		Initialize();

		//Hides UI by default
		TurnOffPanel();
	}

	//initializes variables that should be reset upon scene starting
	void Initialize()
	{
		//sets up button list
		currentButtons = new List<Button>();
	}

	#region Ink Functions

	// Creates a new Story object with the compiled story which we can then play!
	public void StartStory()
	{
		story = new Story(inkJSONAsset.text);
		if (OnCreateStory != null) OnCreateStory(story);

		RefreshView();
	}

	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	public void RefreshView()
	{
		// Remove all the UI on screen
		ResetDialogue();

		// Display all the choices, if there are any!
		if (story.currentChoices.Count > 0)
		{
			//the game state is now choosing
			gm.currentState = GameState.Choosing;

			HideDialogueArrow();

			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView(choice.text.Trim());
				// Tell the button what to do when we press it
				button.onClick.AddListener(delegate {
					OnClickChoiceButton(choice);
				});
				//adds to list of current buttons
				currentButtons.Add(button);
			}

			for(int j = 0; j < currentButtons.Count; j++)
            {
				//creates new navigation settings
				Navigation customNav = new Navigation();
				customNav.mode = Navigation.Mode.Explicit;

				//checks if this isn't the last button in the array
				if (j < currentButtons.Count - 1)
                {
					//sets next button when navigation goes down
					customNav.selectOnDown = currentButtons[j + 1];
                }
				
				//checks if this isn't the first button
				if (j > 0)
                {
					customNav.selectOnUp = currentButtons[j - 1];
                }
				//sets new navigation settings
				currentButtons[j].navigation = customNav;
			}

			//selects first button in the list as the highlighted one
			gm.SelectFirstButton(currentButtons[0].gameObject);
		}

		// Read all the content until we can't continue any more
		if (story.canContinue)
		{
			// Continue gets the next line of the story
			currentText = story.Continue();
			// This removes any white space from the text.
			currentText = currentText.Trim();

			//check tags at current line of dialogue, if there are any
			if (story.currentTags.Count > 0)
				CheckTags();

			// Display the text on screen!
			StartCoroutine("ScrollText");
		}

		// If we've read all the content and there's no choices, the story is finished!
		else
		{
			if (story.currentChoices.Count == 0)
			{
				/*Button choice = CreateChoiceView("End of story.\nRestart?");
				choice.onClick.AddListener(delegate
				{
					StartStory();
				});*/

				EndDialogue();
			}
		}
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		gm.currentState = GameState.Talking;
		RefreshView();
	}

	// Creates a button showing the choice text
	Button CreateChoiceView(string text)
	{
		// Creates the button from a prefab
		Button choice = Instantiate(buttonPrefab) as Button;
		choice.transform.SetParent(ButtonContainer.transform, false);

		// Gets the text from the button prefab
		TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void ResetDialogue()
	{
		//resets button references
		ResetButtonList();
		gm.RemoveFirstButton();

		int childCount = ButtonContainer.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i)
		{
			GameObject.Destroy(ButtonContainer.transform.GetChild(i).gameObject);
		}

		//stops scrolling text;
		StopCoroutine("ScrollText");

		//resets dialogue box
		textObject.text = "";
	}

	//basic end to the dialogue
	void EndDialogue()
	{
		//hides any dialogue UI
		HideDialogueArrow();
		HideUI();

		//plays out any functions when the dialogue ends
		//Don't put any checks for if the player is not talking here because the state changes after these events
		OnDialogueEnd.Invoke();

		//the player is no longer talking
		gm.currentState = GameState.NotTalking;
	}

	//scrolls text
	private IEnumerator ScrollText()
	{
		/*
		//begin scrolling functions
		HideDialogueArrow();
		textObject.text = "";

		foreach (char letter in currentText)
		{
			//adds each letter from the string into the dialogue box and then waits for a bit
			textObject.text += letter;

			//checks if the audio source has a blip to use and if the dialogue audio is not playing
			if (dialogueBlip != null && dialogueAudio.isPlaying == false)
			{
				//plays dialogue blip
				dialogueAudio.PlayOneShot(dialogueBlip);
			}
			yield return new WaitForSeconds(1 - (scrollSpeed / 100));

			//if the current state isn't talking, it sure is now
			//put this here because the text automatically skipped after choosing a button and that didn't quite look right
			if (gm.currentState != GameState.Talking)
				gm.currentState = GameState.Talking;
		}*/

		//clears out current text
		ClearOutText();

		//gets text info
		TMPro.TMP_TextInfo textInfo = textObject.textInfo;

		int visibleCharCount = 0;
		
		//runs through all visible characters
		for (int j = 0; j < textInfo.characterCount; j++)
		{
			if (textInfo.characterInfo[j].isVisible)
            {
				visibleCharCount++;
            }
		}

		//runs through each visible character
		for (int i = 0; i < visibleCharCount; i++)
        {
			//gets reference to text material, vertex positions, and colors
			TMPro.TMP_MeshInfo meshInfo = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex];
			Vector3[] verts = meshInfo.vertices;
			Color32[] colors = meshInfo.colors32;

			//goes through all vertices in character mesh
			for (int k = (i * 4); k < (i * 4) + 4; k++)
			{
				//sets text to opaque
				colors[k] = new Color32(colors[k].r, colors[k].g, colors[k].b, 255);
				textObject.mesh.colors32 = colors;

				textObject.UpdateVertexData();

				//tweens the text into the text box
				StartCoroutine(LerpText(verts[k], k, verts));
			}
			gm.am.PlaySoundNoContextRandomizePitch(gm.am.defaultLetterBlip);
			yield return new WaitForSeconds(1 - (scrollSpeed / 100));
		}

		//end of scrolling functions
		ShowDialogueArrow();
		OnLineEnd.Invoke();
		gm.currentState = GameState.WaitingToAdvance;
	}

	//has the character in the text move in (using vertex)
	IEnumerator LerpText(Vector3 refPos, int index, Vector3[] vertArray)
	{
		//gets starting position of the character based on offset set by the dev
		Vector3 startingPosition = refPos + charPositionOffset;

		//tweens the text flying in
		for (float i = 0; i < tweenDuration; i += Time.deltaTime)
		{
			//evaluates the position of the text depending at what part of the tween it's at (from 0 to 1, which is caluclated by dividing the amount of time passed by the tween's duration)
			float posX = Mathf.Lerp(startingPosition.x, refPos.x, tweenEasingCurve.Evaluate(i / tweenDuration)) + XModification.Evaluate(i / tweenDuration);
			float posY = Mathf.Lerp(startingPosition.y, refPos.y, tweenEasingCurve.Evaluate(i / tweenDuration)) + YModification.Evaluate(i / tweenDuration);

			//creates new vertex position
			Vector3 newVertPos = new Vector3(posX, posY, 0);
			vertArray[index] = newVertPos;

			yield return new WaitForSeconds(Time.deltaTime);

			textObject.UpdateVertexData();

		}

		//snaps vertex to original position at the end of the loop
		vertArray[index] = refPos;
		textObject.UpdateVertexData();
	}

	//sets current vertex colors to transparent
	public void ClearOutText()
    {
		//sets current line
		textObject.text = currentText;
		textObject.ForceMeshUpdate();
		TMPro.TMP_TextInfo textInfo = textObject.textInfo;

		//clears out text (makes it invisible)
		foreach (TMPro.TMP_CharacterInfo characterInfo in textInfo.characterInfo)
		{
			//gets reference to text material, vertex positions, and colors
			TMPro.TMP_MeshInfo meshInfo = textInfo.meshInfo[characterInfo.materialReferenceIndex];
			Vector3[] verts = meshInfo.vertices;
			Color32[] colors = meshInfo.colors32;

			//goes through each character's vertices and makes them transparent
			for (int i = 0; i < verts.Length; i++)
			{
				//sets text to transparent
				meshInfo.colors32[i] = new Color32(colors[i].r, colors[i].g, colors[i].b, 0);
			}

			//actually sets transparent text color
			textObject.textInfo.meshInfo[characterInfo.materialReferenceIndex] = meshInfo;
			textObject.UpdateVertexData();
		}
	}

	//skips the scrolling and fills in finished dialogue
	public void SkipScroll()
	{
		StopAllCoroutines();

		ShowDialogueArrow();

		//resets the text
		textObject.text = currentText;
		textObject.ForceMeshUpdate();
		TMPro.TMP_TextInfo textInfo = textObject.textInfo;

		//makes text visible
		foreach (TMPro.TMP_CharacterInfo characterInfo in textInfo.characterInfo)
		{
			//gets reference to text material, vertex positions, and colors
			TMPro.TMP_MeshInfo meshInfo = textInfo.meshInfo[characterInfo.materialReferenceIndex];
			Vector3[] verts = meshInfo.vertices;
			Color32[] colors = meshInfo.colors32;

			//goes through each character's vertices and makes them fully opaque
			for (int i = 0; i < verts.Length; i++)
			{
				//sets vertex to opaque
				meshInfo.colors32[i] = new Color32(colors[i].r, colors[i].g, colors[i].b, 255);
			}

			//actually sets text color
			textObject.textInfo.meshInfo[characterInfo.materialReferenceIndex] = meshInfo;
			textObject.UpdateVertexData();

		}

		//calls on line end functions
		OnLineEnd.Invoke();
		gm.currentState = GameState.WaitingToAdvance;
	}

	#endregion

	#region Tag System

	//checks tags of current line of dialogue for events
	void CheckTags()
	{
		//runs through all current tags
		foreach (string tag in story.currentTags)
		{

			//this method allows anyone to edit what happens when a tag is called in the inspector
			//runs through each tag in the tag list
			foreach(Tag tagCheck in Tags)
			{
				//checks if the tag contains a certain marker from the tag list
				if (tag.Contains(tagCheck.marker))
				{
					//calls whatever method(s) occur when this tag is called and passes the tag data through
					tagCheck.TagFunctions.Invoke(GetTagData(tag));
					//stops the loop
					break;
				}
			}
		}
	}

	//takes data from the marked tag and returns it as a value
	string GetTagData(string tag)
	{
		//removes the marking from the string in the tag
		string tagData = tag.Remove(0, 1);

		if(!string.IsNullOrEmpty(tagData))
			return tagData;

        else
        {
			return (null);
        }
	}

	//reads info from tag that only has one element
	public string ReadTagInfoSingle(string tag)
	{
		return (tag.Remove(0, 1));
	}

	public void TestTag(string tagData)
	{
		print("this tag has data of " + tagData);
	}

	//goes to specified story branch
	void SelectNewStoryBranch(string branchToGoTo)
	{
		story.ChoosePathString(branchToGoTo);
	}

	#endregion

	#region UI Controls

	//shows UI panel
	public void ShowUI()
	{
		UIPanel.SetActive(true);
		PanelAnimation.Play("SpeechBubble_Ani_Show");
	}

	//hides UI panel
	public void HideUI()
	{
		PanelAnimation.Play("SpeechBubble_Ani_Hide");
	}

	//disables UI Panel
	public void TurnOffPanel()
    {
		UIPanel.SetActive(false);
	}

	//clears out current buttons
	void ResetButtonList()
	{
		currentButtons.Clear();
	}

	//shows dialogue arrow
	void ShowDialogueArrow()
	{
		DialogueArrow.enabled = true;
	}

	//hides dialogue arrow
	void HideDialogueArrow()
	{
		DialogueArrow.enabled = false;
	}

	//moves the worldspace canvas
	public void MoveCanvas(Transform refPoint)
    {
		StartCoroutine(TweenCanvas(refPoint));
    }

	//actually moves the canvas via a tween
	IEnumerator TweenCanvas(Transform reference)
    {
		Vector3 targetPos = new Vector3(reference.position.x, reference.position.y, WorldSpaceCanvas.transform.position.z);

		for (float i = 0; i < SpeechBubbleMoveDuration; i += Time.deltaTime)
        {
			WorldSpaceCanvas.transform.position = Vector3.Lerp(WorldSpaceCanvas.transform.position, targetPos, i / SpeechBubbleMoveDuration);

			yield return new WaitForSeconds(Time.deltaTime);
        }
    }

	//moves canvas over the current NPC
	public void MoveCanvasToNPC()
    {
		MoveCanvas(gm.currentNPC.speechBubbleRefPoint.transform);
    }

	public void InitializeCanvasLocation()
    {
		//creates a reference to just the first line of text
		Story storyRef = new Story(inkJSONAsset.text);
		// Continue gets the next line of the story
		string firstLine = storyRef.Continue();
		// This removes any white space from the text.
		firstLine = firstLine.Trim();

		foreach(string tag in storyRef.currentTags)
        {
			if(tag.Contains(speechBubblePlayerTag))
            {
				foreach(Tag playerTag in Tags)
                {
					if (playerTag.marker.Contains(speechBubblePlayerTag))
                    {
						playerTag.TagFunctions.Invoke(GetTagData(speechBubblePlayerTag));
						break;
					}
                }
            }
			
			else if (tag.Contains(speechBubbleNPCTag))
            {
				foreach (Tag npcTag in Tags)
				{
					if (npcTag.marker.Contains(speechBubbleNPCTag))
					{
						npcTag.TagFunctions.Invoke(GetTagData(speechBubbleNPCTag));
						break;
					}
				}
			}
        }

	}

	#endregion

	//Checks which state the game is in for input
	public virtual void CheckInteractionInput()
	{
		switch (gm.currentState)
		{
			case GameState.NotTalking:
				//checks if there is an npc to talk to
				if (gm.currentNPC != null)
				{
					gm.currentState = GameState.Talking;

					//calls any functions that are supposed to happen when the dialogue begins
					OnDialogueStart.Invoke();

					GetNPCTextAsset();
					ShowUI();
				}

				break;

			case GameState.Talking:
				SkipScroll();
				break;

			case GameState.WaitingToAdvance:
				gm.currentState = GameState.Talking;
				RefreshView();
				gm.am.PlaySoundNoContext(gm.am.defaultLineBlip);
				break;
		}
	}

	//gets text from current npc (is used as a base)
	public virtual void GetNPCTextAsset()
	{
		//Sets dialogue based on what arc the game is currently on
		inkJSONAsset = gm.currentNPC.GetCurrentText(gm.arc);

		//increments number of times the player has spoken to this npc
		gm.currentNPC.timesSpokenTo++;
	}

	//gets dialogue index at this point in the story
	int GetDialogueIndex(NPCScript npc)
    {
		int index = 0;

		//checks if there is NO more dialogue after this arc
		if (gm.arc >= npc.dialogueArcs.Length - 1)
		{
			//index = npc.dialogueArcs[npc.dialogueArcs.Length - 1].dialogueSets[npc.timesSpokenTo];

		}
		//checks if there IS dialogue after this arc
		else
		{
			//index = npc.dialogueArcs[gm.arc].dialogueSets[npc.timesSpokenTo];
		}

		return index;
    }

	public GameManager gm;

	public TextAsset inkJSONAsset = null;
	public Story story;
	private string currentText;

	[SerializeField]
	private RectTransform ButtonContainer = null;

	// UI Prefabs
	[Header("UI Objects")]
	[SerializeField]
	private TextMeshProUGUI textObject = null;
	[SerializeField]
	private GameObject UIPanel;
	[SerializeField]
	private Animation PanelAnimation;
	[SerializeField]
	private Button buttonPrefab = null;
	public List<Button> currentButtons;
	public Image DialogueArrow;
	public GameObject WorldSpaceCanvas;
	public float SpeechBubbleMoveDuration;
	public string speechBubblePlayerTag, speechBubbleNPCTag;

	[Header("Text Scroll")]
	public AudioSource dialogueAudio;
	public AudioClip dialogueBlip;
	[Range(90, 100)]
	public float scrollSpeed = 90f;

	[Header("Text Scroll Tweening")]
	[SerializeField]
	AnimationCurve XModification;
	[SerializeField]
	AnimationCurve YModification;
	[SerializeField]
	AnimationCurve tweenEasingCurve;
	[SerializeField]
	float tweenDuration;
	[SerializeField]
	Vector3 charPositionOffset;

	[Header("Events")]
	public UnityEvent OnDialogueStart;
	public UnityEvent OnLineEnd;
	public UnityEvent OnDialogueEnd;

	[Header("Tag List")]
	public Tag[] Tags;

	[System.Serializable]
	public struct Tag {
		public string marker;
		public TagFunctions TagFunctions;
	}
	
	
}

//This class is only here so that an event that takes a string value parameter shows up in the inspector
[System.Serializable]
public class TagFunctions : UnityEvent<string>
{
}

/*
 *TO DO
 * Sound that plays when going to next dialogue line
 * 
 * ASK LATER
*/
