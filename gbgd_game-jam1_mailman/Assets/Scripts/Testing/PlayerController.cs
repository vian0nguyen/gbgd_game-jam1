using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    protected GameManager gm;

    public UnityEvent OnInteractButtonDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //checks if player presses the spacebar
        if (Input.GetButtonDown("Submit"))
        {
            //plays all events from when the interaction button gets hit
            OnInteractButtonDown.Invoke();
        }
        
    }
}
