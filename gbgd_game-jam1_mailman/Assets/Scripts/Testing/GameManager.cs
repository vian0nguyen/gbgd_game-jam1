using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Game States")]
    public GameState currentState;
    public bool moveToNextArc;
    public bool endGame;

    #region Inventory
    [Header ("Inventory")]
    [SerializeField]
    public Dictionary<string, Item> inventory;
    public int maxCapacity = 3;
    public List<string> PlayerInventory;
    [System.Serializable]
    public struct Item
    {
        public string objectName;
    }
    #endregion

    [Header("UI")]
    [SerializeField]
    private EventSystem es;

    public int arc;

    public NPCScript currentNPC = null;

    private void Awake()
    {
        //sets up blank inventory
        inventory = new Dictionary<string, Item>();
        PlayerInventory = new List<string>();
    }


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
        moveToNextArc = false;
        endGame = false;

        #region Arc Setup
#if UNITY_EDITOR
        //if the code is in the editor, the arc is set to whatever the dev wants for playtest purposes
        print("In Editor");
#else
        //if the code is in a build, automatically sets the arc to the beginning
        arc = 0;
        print("Not in Editor");
#endif
        #endregion
    }

    //allows easier setting of game state (not from code though) (used in editor and animation events) (see guide below on what is what)
    public void SetGameState(int state)
    {
        currentState = (GameState)state;
    }

    #region Inventory Functions
    //adds mail to player's inventory
    public void AddToInventory(string itemName)
    {
        //checks if there is a name to read
        if (!string.IsNullOrEmpty(itemName))
        {
            //checks if there is enough room in the player's inventory to add something
            if (inventory.Count < maxCapacity)
            {
                inventory.Add(itemName, new Item());
                print("Added " + itemName);

                //only runs in editor
#if UNITY_EDITOR
                PlayerInventory.Add(itemName);
#endif
            }

            else
            {
                //show dialogue that you can't hold anymore items
            }
        }

        else
        {
            Debug.LogError("Please add a valid name after the tag marker");
        }
    }

    //removes mail from inventory
    public void RemoveFromInventory(string itemName)
    {
        //checks if there is a name to read
        if (!string.IsNullOrEmpty(itemName))
        {
            //checks if the player had anything to remove
            if (inventory.Count > 0)
            {
                inventory.Remove(itemName);
                print("Removed " + itemName);

#if UNITY_EDITOR
                PlayerInventory.Remove(itemName);
#endif
            }

            else
            {
                //show dialogue that you don't have anything in your inventory
            }
        }

        else
        {
            Debug.LogError("Please add a valid name after the tag marker");
        }
    }

    #endregion

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

    //at the end of the dialogue, a new arc will start
    public void EnableNewArc()
    {
        moveToNextArc = true;
    }

    //sets new arc
    public void SetNewArc()
    {
        //checks if we're able to move to the next arc
        if (moveToNextArc)
        {
            arc++;
            moveToNextArc = false;
        }
    }

    public void PlayEndSequence()
    {
        if (endGame)
        {

        }
    }

    //resets the game by loading the current scene
    public void ResetGame()
    {
        int sceneNum = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneNum);
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
#region Game States
public enum GameState { 
        isBeginning = 0, 
        NotTalking = 1,
        Talking = 2,
        WaitingToAdvance = 3,
        Choosing = 4,
        isTransitioning = 5,
        isEnding = 6 
 };
#endregion