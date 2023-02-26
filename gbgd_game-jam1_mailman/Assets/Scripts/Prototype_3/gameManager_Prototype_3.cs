using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager_Prototype_3 : GameManager
{

    private void Awake()
    {
        //sets up blank inventory
        inventory = new Dictionary<string, Item>();
        PlayerInventory = new List<string>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

}
//create scriptables for objects that list their correct recipient and check names, regardless of case
//figure out how to apply data from previous text when arc changes (bools?)