using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameManager gm;
    public Dialogue_Writer dw;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //checks if player presses the spacebar
        if (Input.GetButtonDown("Jump"))
        {
            CheckInteractionInput();
        }
        
    }

    //Checks which state the game is in for input
    void CheckInteractionInput()
    {
        switch (gm.currentState)
        {

            case GameManager.GameState.NotTalking:
                gm.currentState = GameManager.GameState.Talking;
                dw.ShowUI();
                dw.StartStory();
                break;

            case GameManager.GameState.Talking:
                dw.SkipScroll();
                break;

            case GameManager.GameState.WaitingToAdvance:
                dw.RefreshView();
                break;


        }
    }

}
