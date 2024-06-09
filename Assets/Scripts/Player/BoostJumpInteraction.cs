using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostJumpInteraction : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float interactionDistance = 0.2f;
    public float liftedHeight = 0.5f;
    private bool isPlayer1Lifted = false;

    public CameraSwitchController cameraSwitchController;
    public GameObject interactionUI;
    public GameObject interactionJumpUI;


    void Start()
    {
        
        player1.GetComponent<PlayerMovement>().onJump.AddListener(OnPlayer1Jump);
    }

    void Update()
    {
        if (cameraSwitchController.currentCamera.GetComponent<CameraFollow>().target != player1) return;

        float distance = Vector3.Distance(player1.position, player2.position);

        if (distance <= interactionDistance)
        {
            interactionUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                if (!isPlayer1Lifted)
                {
                    interactionJumpUI.SetActive(true);
                    LiftPlayer1();
                }
                else
                {
                    interactionJumpUI.SetActive(false);
                    DropPlayer1();
                }
            }
        } else
        {
            interactionUI.SetActive(false);
            interactionJumpUI.SetActive(false);
        }
    }

    void LiftPlayer1()
    {
        player1.position += Vector3.up * liftedHeight;
        player1.GetComponent<PlayerMovement>().BoostJump(true);
        player1.GetComponent<PlayerMovement>().SetLifted(true);
        isPlayer1Lifted = true;
    }

    void DropPlayer1()
    {
        player1.GetComponent<PlayerMovement>().BoostJump(false);
        player1.GetComponent<PlayerMovement>().SetLifted(false);
        isPlayer1Lifted = false;
    }

    void OnPlayer1Jump()
    {
        isPlayer1Lifted = false;
    }
}
