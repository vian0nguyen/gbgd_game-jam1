﻿using System.Collections;
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
    //remember to nullify this after buttons are destroyed
    public void SelectFirstButton(GameObject firstButton)
    {
        es.firstSelectedGameObject = firstButton;

        //remember the color used for this highlight is "Selected Color"
    }

    //removes first selected gameObject
    public void RemoveFirstButton()
    {
        es.firstSelectedGameObject = null;
    }
}