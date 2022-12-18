using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//this script's function is to trigger events in an animation clip that doesn't have the affected objects as children
public class animationEventHandler : MonoBehaviour
{
    public UnityEvent[] AnimationEvents;
    
    //all this does is trigger the listed functions from other scripts
    public void TriggerEvent(int eventIndex)
    {
        AnimationEvents[eventIndex].Invoke();
    }
}
