using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager_Prototype_1 : GameManager
{
    #region Inventory
    public Dictionary<string, Item> inventory;
    public int maxCapacity = 3;
    public struct Item
    {

    }
    #endregion

    public int arc;

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
        //checks if there is enough room in the player's inventory to add something
        if (inventory.Count < maxCapacity)
        {
            inventory.Add(itemName, new Item());
        }

        else
        {
            //show dialogue that you can't hold anymore items
        }
    }

    //removes mail from inventory
    public void RemoveFromInventory(string itemName)
    {
        //checks if the player had anything to remove
        if (inventory.Count > 0)
        {
            inventory.Remove(itemName);
        }

        else
        {
            //show dialogue that you don't have anything in your inventory
        }
    }

}
