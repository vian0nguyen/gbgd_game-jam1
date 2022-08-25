using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public enum GameState { NotTalking, Talking, WaitingToAdvance, Choosing };
    [Header("Game States")]
    public GameState currentState;

    [Header("UI")]
    [SerializeField]
    private EventSystem es;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        RemoveFirstButton();
    }

    //initializes variables for this scene
    void Initialize()
    {
        currentState = GameState.NotTalking;
    }

    //Sets first button spawned as hovered over in the event system
    public void SelectFirstButton(GameObject firstButton)
    {
        es.SetSelectedGameObject(firstButton);

        //remember the color used for this highlight is "Selected Color"
    }

    //removes first selected gameObject
    public void RemoveFirstButton()
    {
        es.SetSelectedGameObject(null);
    }

    #region Test Functions
    public void EndLine()
    {
        print("The line has ended!");
    }

    public void EndDialogue()
    {
        print("The dialogue has ended!");
    }

    public void TagTest()
    {
        print("A certain tag was just called");
    }

    #endregion
}
