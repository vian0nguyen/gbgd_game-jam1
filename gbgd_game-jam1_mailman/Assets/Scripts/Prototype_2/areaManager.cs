using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaManager : MonoBehaviour
{

    public GameObject[] areas;
    public GameObject startingArea;
    public int currentAreaIndex;
    public Animation fade;
    public AnimationClip fadeAnimationClip;

    // Start is called before the first frame update
    void Start()
    {
         initializeArea();
    }

    //finds the correct area to turn on
    public void TurnOnArea()
    {
       for (int i = 0; i < areas.Length; i++)
        {
            //turns on designated area
            if(i == currentAreaIndex)
            {
                areas[i].SetActive(true);
            }
            else
            {
                areas[i].SetActive(false);
            }
        }
    }

    //sets up starting area
    void initializeArea()
    {
        for (int i = 0; i < areas.Length; i++)
        {
            if(areas[i] == startingArea)
            {
                currentAreaIndex = i;
                break;
            }
        }
        TurnOnArea();
    }

    //goes to upper area
    public void AreaUp()
    {
        currentAreaIndex++;
        fade.Play(fadeAnimationClip.name);
    }

    //goes to lower area
    public void AreaDown()
    {
        currentAreaIndex--;
        fade.Play(fadeAnimationClip.name);
    }
}
