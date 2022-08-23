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
            

        }
        
    }

    void CheckInteractionInput()
    {
        switch (gm.currentState)
        {

            case GameManager.GameState.NotTalking:
                dw.StartStory();
                break;

            case GameManager.GameState.WaitingToAdvance:
                dw.RefreshView();
                break;

            //case GameManager.GameState.Choosing:



        }
    }

}
