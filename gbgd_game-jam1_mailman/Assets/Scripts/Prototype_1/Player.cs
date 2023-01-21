using System.Collections;
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

    List<GameObject> NPCsInRange = new List<GameObject>();
    gameManager_Prototype_1 gmp1;

    private void Awake()
    {
        //checks if the gameManager is the one specifically for prototype 1
        if (gm is gameManager_Prototype_1)
            gmp1 = gm as gameManager_Prototype_1;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        GetMovementInput();
    }

    void GetMovementInput()
    {
        //checks if the player isn't talking
        if (gm.currentState == GameManager.GameState.NotTalking)
        {
            //gets input from the player
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
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
        if(gm.currentState == GameManager.GameState.NotTalking)
        {
            //sets player's velocity
            rb2D.velocity = new Vector2(moveX * speed, moveY * speed);
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

                //checks if this is first prototype
                if (gm is gameManager_Prototype_1)
                    gmp1.currentNPC = NPCsInRange[NPCsInRange.Count - 1];

                //checks if this is 3rd prototype (will clean this up with a more finalized version)
                else if (gm is gameManager_Prototype_3)
                {
                    gameManager_Prototype_3 gmp3 = gm as gameManager_Prototype_3;
                    gmp3.currentNPC = NPCsInRange[NPCsInRange.Count - 1];
                }

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
                    if (gm is gameManager_Prototype_1)
                        gmp1.currentNPC = NPCsInRange[NPCsInRange.Count - 1];

                    else if (gm is gameManager_Prototype_3)
                    {
                        gameManager_Prototype_3 gmp3 = gm as gameManager_Prototype_3;
                        gmp3.currentNPC = NPCsInRange[NPCsInRange.Count - 1];
                    }
                }

                //if the player is in the range of another npc, the current npc is the last one that the player entered the talking range of
                else
                {
                    if (gm is gameManager_Prototype_1)
                        gmp1.currentNPC = null;

                    else if (gm is gameManager_Prototype_3)
                    {
                        gameManager_Prototype_3 gmp3 = gm as gameManager_Prototype_3;
                        gmp3.currentNPC = null;
                    }

                }
                break;
        }
    }
    #endregion
}
