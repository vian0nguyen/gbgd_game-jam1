﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerController
{
    #region Controls
    public Rigidbody2D rb2D;
    public float speed;
    float moveX;
    float moveY;
    #endregion

    #region Transitions
    public float verticalTransitionThreshold;
    public bool canTransition;
    public areaManager am;
    [SerializeField]
    private TransitionLimitScript currentTransitionData;
    public bool canMovePlayer;
    #endregion

    List<GameObject> NPCsInRange = new List<GameObject>();

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        GetInput();

        
    }

    //input list for different states in the game
    void GetInput()
    {
        switch (gm.currentState)
        {
            //checks if the player is at the beginning of the game
            case GameState.isBeginning:
                break;
            
            //checks if the player is in the ending of the game
            case GameState.isEnding:
                GetResetInput();
                break;
            
            //checks if the player is not talking at the moment
            case GameState.NotTalking:
                GetMovementInput();
                break;
            
            //checks if the player is transitioning
            case GameState.isTransitioning:
                break;
            default:
                break;
        }
    }

    void GetMovementInput()
    {
        //gets input from the player
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        //checks if there's any vertical input
        if (canTransition && moveY != 0)
        {
            Transition(moveY);
        }

    }

    //gets input for resetting the game (if the player presses any key
    void GetResetInput()
    {
        if (Input.anyKeyDown && gm.currentState == GameState.isEnding)
        {
            gm.ResetGame();
        }
    }

    #region Physics
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        //checks if the player isn't talking
        if(gm.currentState == GameState.NotTalking)
        {
            //sets player's velocity
            rb2D.velocity = new Vector2(moveX * speed, 0);
        }
    }
    
    //freezes the player's velocity so that they don't go sliding offscreen
    public void FreezePlayer()
    {
        rb2D.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag){
            
            //checks if the player is in an NPC's talking range
            case "NPC":
                NPCsInRange.Add(collision.gameObject);
                gm.currentNPC = NPCsInRange[NPCsInRange.Count - 1].GetComponent<NPCScript>();
                break;

            //checks if the player collides with a transition object
            case "transition":

                canTransition = true;
                currentTransitionData = collision.gameObject.GetComponent<TransitionLimitScript>();
                break;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {

            //checks if the player leaves an NPC's talking range
            case "NPC":
                NPCsInRange.Remove(collision.gameObject);
                
                //if the player leaves an NPC's talking range and there are no overlapping NPC's, the current npc is nullified
                if(NPCsInRange.Count > 0)
                {
                    gm.currentNPC = NPCsInRange[NPCsInRange.Count - 1].GetComponent<NPCScript>();
                }

                //if the player is in the range of another npc, the current npc is the last one that the player entered the talking range of
                else
                {
                    gm.currentNPC = null;
                }
                break;

            //checks if the player collides with a transition object
            case "transition":

                canTransition = false;
                currentTransitionData = null;
                break;
                
        }
    }
    #endregion

    #region Transition Functions
    //moves between areas
    void Transition(float input)
    {

        rb2D.velocity = Vector2.zero;

        //checks if the player is pressing all the way up
        if (input > verticalTransitionThreshold && am.currentAreaIndex < am.areas.Length - 1)
        {
            //checks to see if this current road is not a dead end
            if (currentTransitionData.paths[am.currentAreaIndex].pathConnect == TransitionLimitScript.connectionType.Both || currentTransitionData.paths[am.currentAreaIndex].pathConnect == TransitionLimitScript.connectionType.Bottom)
            {
                gm.currentState = GameState.isTransitioning;
                am.AreaUp();
                currentTransitionData.ChangeSprite(am.currentAreaIndex);
            }

        }

        //checks if the player is pressing all the way down
        else if (input < -verticalTransitionThreshold && am.currentAreaIndex > 0)
        {
            //checks to see if this current road is not a dead end
            if (currentTransitionData.paths[am.currentAreaIndex].pathConnect == TransitionLimitScript.connectionType.Both || currentTransitionData.paths[am.currentAreaIndex].pathConnect == TransitionLimitScript.connectionType.Top)
            {
                gm.currentState = GameState.isTransitioning;
                am.AreaDown();
                currentTransitionData.ChangeSprite(am.currentAreaIndex);
            }
        }
    }

    //resets player's ability to move, and makes sure they can't transition unless they go to another transition point or re enter the current one
    public void ResetPlayer()
    {
        gm.currentState = GameState.NotTalking;
        canTransition = false;
    }

    //warps the player to wherever we want based on a TAG
    public void WarpPlayer()
    {
        if (canMovePlayer)
        {
            am.fade.Play(am.fadeAnimationClip.name);
            canMovePlayer = false;
        }
    }

    //sets new area number and sets condition to warp player at end of dialogue
    public void SetupWarpPlayer(string areaNumber)
    {

        if (int.TryParse(areaNumber, out int tempAreaNum) && int.Parse(areaNumber) > 0 && int.Parse(areaNumber) < am.areas.Length)
            am.currentAreaIndex = tempAreaNum;
        else
            Debug.LogError("Please enter a valid area number");

        canMovePlayer = true;
    }
    #endregion
}
