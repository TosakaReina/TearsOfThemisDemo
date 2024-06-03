using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    public CameraFollow cameraFollow;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TogglePlayerMovement();
        }
    }

    private void TogglePlayerMovement()
    {
        playerMovementScript.enabled = !playerMovementScript.enabled;
        cameraFollow.enabled = !cameraFollow.enabled;
    }
}

