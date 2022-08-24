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

	void Awake()
	{
		//sets up variables to be reset upon the scene starting
		Initialize();

		//Hides UI by default
		HideUI();
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
			gm.currentState = GameManager.GameState.Choosing;

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
				HideDialogueArrow();
				HideUI();

				//plays out any functions when the dialogue ends
				OnDialogueEnd.Invoke();
				//play sound here?

				gm.currentState = GameManager.GameState.NotTalking;
			}
		}

		//check tags at current line of dialogue, if there are any
		if (story.currentTags.Count > 0)
			CheckTags();
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
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

	//scrolls text
	private IEnumerator ScrollText()
	{
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
			if (gm.currentState != GameManager.GameState.Talking)
				gm.currentState = GameManager.GameState.Talking;
		}

		//end of scrolling functions
		ShowDialogueArrow();
		OnLineEnd.Invoke();
		gm.currentState = GameManager.GameState.WaitingToAdvance;
	}

	//skips the scrolling and fills in finished dialogue
	public void SkipScroll()
	{
		StopCoroutine("ScrollText");
		ShowDialogueArrow();
		textObject.text = currentText;
		
		//calls on line end functions
		OnLineEnd.Invoke();
		gm.currentState = GameManager.GameState.WaitingToAdvance;
	}

    #endregion

    #region Tag System

	//checks tags of current line of dialogue for events
	void CheckTags()
	{
		foreach(string tag in story.currentTags)
		{
			switch (tag) {
				case string a when a.Contains("~"):
					print("has ~ with data of " + GetTagData(tag));
					break;
			}
		}
	}

    //goes to specified story branch
    void SelectNewStoryBranch(string branchToGoTo)
	{
		story.ChoosePathString(branchToGoTo);
	}

	//takes data from the marked tag
	string GetTagData(string tag)
	{
		//removes the marking from the string in the tag
		string tagName = tag.Remove(0, 1);

		return tagName;
	}

	#endregion

	#region UI Controls

	//shows UI panel
	public void ShowUI()
	{
		UIPanel.SetActive(true);
	}

	//hides UI panel
	public void HideUI()
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

	//Checks which state the game is in for input
	public void CheckInteractionInput()
	{
		switch (gm.currentState)
		{
			case GameManager.GameState.NotTalking:
				gm.currentState = GameManager.GameState.Talking;
				ShowUI();
				StartStory();
				break;

			case GameManager.GameState.Talking:
				SkipScroll();
				break;

			case GameManager.GameState.WaitingToAdvance:
				RefreshView();
				break;
		}
	}

	#endregion

	//player checks to advance the dialogue is called on button press

	[SerializeField]
	private GameManager gm;

	[SerializeField]
	private TextAsset inkJSONAsset = null;
	public Story story;
	private string currentText;

	[SerializeField]
	private RectTransform ButtonContainer = null;

	// UI Prefabs
	[Header ("UI Objects")]
	[SerializeField]
	private TextMeshProUGUI textObject = null;
	[SerializeField]
	private GameObject UIPanel;
	[SerializeField]
	private Button buttonPrefab = null;
	private List<Button> currentButtons;
	public Image DialogueArrow;

	[Header("Text Scroll")]
	public AudioSource dialogueAudio;
	public AudioClip dialogueBlip;
	[Range(90, 100)]
	public float scrollSpeed = 90f;

	[Header("Events")]
	public UnityEvent OnLineEnd;
	public UnityEvent OnDialogueEnd;
}

/*
 *TO DO
 * End of dialogue events (delegates?)
 * Format Ink file and document how to write it (find a way to incorporate a lot of Ink's functionality while making it work with the project at hand, though some stuff will get omitted due to how we're displaying text)
 * Sound that plays when going to next dialogue line
 * 
 * ASK LATER
 * Scriptable objects for cutscenes? (if we're having cutscenes at all) (if so, place possible tag in next line of dialogue, hide ui, play out cutscene, then use animation event to continue dialogue [refreshView]) (organize cutscene assets in arrays and just go down the list [no need to write anything in the tag])
 * come up with interesting way to read people's mail lmao (TEXT EFFECTS??)
 * are we having the text repeat itself after pressing a button? (feels a little redundant but idk)
*/
