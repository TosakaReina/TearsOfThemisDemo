using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public PlayerMovement playerMovementScript;

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
    }
}

