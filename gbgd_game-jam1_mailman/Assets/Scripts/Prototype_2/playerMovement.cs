using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    float moveX;
    float moveY;
    public float verticalTransitionThreshold;
    public Rigidbody2D rb2D;

    public enum moveState {isMoving, isTransitioning}
    public moveState state;
    public bool canTransition;
    public areaManager am;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
    }

    void GetMovementInput()
    {
        //checks if the player isn't talking
        if (state == playerMovement.moveState.isMoving)
        {
            //gets input from the player
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
            
            //checks if there's any vertical input
            if(canTransition && moveY != 0)
            {
                Transition(moveY);
            }
        }

    }

    //moves between areas
    void Transition(float input)
    {

        rb2D.velocity = Vector2.zero;

        //checks if the player is pressing all the way up
        if(input > verticalTransitionThreshold && am.currentAreaIndex < am.areas.Length - 1)
        {
            state = playerMovement.moveState.isTransitioning;
            am.AreaUp();

        }
        
        //checks if the player is pressing all the way down
        else if (input < -verticalTransitionThreshold && am.currentAreaIndex > 0)
        {
            state = playerMovement.moveState.isTransitioning;
            am.AreaDown();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        //checks if the player isn't talking
        if (state == moveState.isMoving)
        {
            //sets player's velocity
            rb2D.velocity = new Vector2(moveX * speed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks if the player collides with a transition object
        if(other.gameObject.tag == "transition")
        {
            canTransition = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //checks if the player collides with a transition object
        if (other.gameObject.tag == "transition")
        {
            canTransition = false;
        }
    }

    //resets player's ability to move, and makes sure they can't transition unless they go to another transition point or re enter the current one
    public void ResetPlayer()
    {
        state = playerMovement.moveState.isMoving;
        canTransition = false;
    }

}
