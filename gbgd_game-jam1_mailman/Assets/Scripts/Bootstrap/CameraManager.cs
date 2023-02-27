using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameManager gm;

    [Header("Cameras")]
    public GameObject playerCam;

    [Header("Target Groups")]
    public CinemachineTargetGroup dialogueTargetGroup;

    //sets camera to player
    public void EnablePlayerCam(GameObject camToTurnOff)
    {
        camToTurnOff.SetActive(false);
        playerCam.SetActive(true);
    }

    //sets camera to something else
    public void TurnOnDifferentCam(GameObject camToTurnOn)
    {
        playerCam.SetActive(false);
        camToTurnOn.SetActive(true);
    }

    //adds current npc into view
    public void AddNPCToTG()
    {
        dialogueTargetGroup.AddMember(gm.currentNPC.gameObject.transform, 1, 1);
    }

    //removes npc from view
    public void RemoveNPCFromTG()
    {
        dialogueTargetGroup.RemoveMember(gm.currentNPC.gameObject.transform);
    }
}
