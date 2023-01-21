using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager_Prototype_3 : GameManager
{
    #region Inventory
    [SerializeField]
    public Dictionary<string, Item> inventory;
    public int maxCapacity = 3;
    public List <string> PlayerInventory;
    [System.Serializable]
    public struct Item
    {
        public string objectName;
    }
    #endregion

    public int arc;

    public GameObject currentNPC = null;

    private void Awake()
    {
        //sets up blank inventory
        inventory = new Dictionary<string, Item>();
        PlayerInventory = new List<string>();
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        //if the code is in the editor, the arc is set to whatever the dev wants for playtest purposes
        print("In Editor");
#else
        //if the code is in a build, automatically sets the arc to the beginning
        arc = 0;
        print("Not in Editor");
#endif
    }

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

    //advances time in the game
    public void IncrementArc()
    {
        arc++;
    }

}
//create scriptables for objects that list their correct recipient and check names, regardless of case
//figure out how to apply data from previous text when arc changes (bools?)