using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;

public class Dialogue_Writer : MonoBehaviour
{
	public static event Action<Story> OnCreateStory;

	void Awake()
	{
		//sets up variables to be reset upon the scene starting
		Initialize();

		// Remove the default message
		ResetDialogue();
		StartStory();
	}

	//initializes variables that should be reset upon scene starting
	void Initialize()
	{
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

		// Read all the content until we can't continue any more
		if (story.canContinue)
		{
			// Continue gets the next line of the story
			currentText = story.Continue();
			// This removes any white space from the text.
			currentText = currentText.Trim();

			//checks if there is any text to scroll and if the player is choosing
			if (currentText.Split(' ').Length > 0 && gm.currentState == GameManager.GameState.Talking)
			{

				// Display the text on screen!
				StartCoroutine("ScrollText");

			}
		}

		// Display all the choices, if there are any!
		if (story.currentChoices.Count > 0)
		{
			//the game state is now choosing
			gm.currentState = GameManager.GameState.Choosing;

			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView(choice.text.Trim());
				// Tell the button what to do when we press it
				button.onClick.AddListener(delegate {
					OnClickChoiceButton(choice);
				});
			}
		}

		// If we've read all the content and there's no choices, the story is finished!
		/*else
		{
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate {
				StartStory();
			});
		}*/
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView(string text)
	{
		TextMeshPro storyText = Instantiate(textPrefab) as TextMeshPro;
		storyText.text = text;
		storyText.transform.SetParent(canvas.transform, false);
	}

	// Creates a button showing the choice text
	Button CreateChoiceView(string text)
	{
		// Creates the button from a prefab
		Button choice = Instantiate(buttonPrefab) as Button;
		choice.transform.SetParent(canvas.transform, false);

		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text>();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void ResetDialogue()
	{
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i)
		{
			GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
		}

		//stops scrolling text;
		StopCoroutine("ScrollText");
	}

	#endregion

	//scrolls text
	private IEnumerator ScrollText()
	{

		foreach (char letter in currentText)
		{

			//adds each letter from the string into the dialogue box and then waits for a bit
			textPrefab.text += letter;

			//checks if the audio source has a blip to use
			if (dialogueBlip != null)
			{

				//checks if the audio source is not playing
				if (dialogueAudio.isPlaying == false)
				{

					//plays the blip as the dialogue scrolls
					dialogueAudio.PlayOneShot(dialogueBlip);

				}

			}

			yield return new WaitForSeconds(1 - (scrollSpeed / 100));

		}

		//turns on arrow UI to advance
		DialogueArrow.enabled = true;

	}

	//goes to specified story branch
	void SelectNewStoryBranch(string branchToGoTo)
	{
		story.ChoosePathString(branchToGoTo);
	}

	//player checks to advance the dialogue is called on button press

	[SerializeField]
	private GameManager gm;

	[SerializeField]
	private TextAsset inkJSONAsset = null;
	public Story story;
	private string currentText;

	[SerializeField]
	private Canvas canvas = null;

	// UI Prefabs
	[SerializeField]
	private TextMeshPro textPrefab = null;
	[SerializeField]
	private Button buttonPrefab = null;
	public Image DialogueArrow;
	[Header ("Buttons")]
	public List<Button> currentButtons;

	[Header("Text Scroll")]
	public AudioSource dialogueAudio;
	public AudioClip dialogueBlip;
	public float scrollspeed;
	[Range(90, 100)]
	public float scrollSpeed = 90f;
}
