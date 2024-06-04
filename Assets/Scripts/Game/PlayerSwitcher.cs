using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public Transform player1; 
    public Transform player2; 
    public CameraFollow cameraFollow;

    private Transform currentPlayer; // current player's transform
    private int currentPlayerIndex = 1; 



    void Start()
    {
        // initialize player 1 as current player
        currentPlayer = player1;
        cameraFollow.SetTarget(currentPlayer); 
        EnablePlayerComponents(currentPlayer); 
        DisablePlayerComponents(player2); // disable other player's Components
    }

    void Update()
    {
        // switch player if press C and Map is not opened
        if (Input.GetKeyDown(KeyCode.C) && !MapMovementController.IsMapOpened)
        {
            SwitchPlayer();
        }

        // toggle current player's movement script
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            TogglePlayerMovement();
        }
    }

    private void TogglePlayerMovement()
    {
        PlayerMovement playerMovementScript = currentPlayer.GetComponent<PlayerMovement>();
        playerMovementScript.enabled = !playerMovementScript.enabled;
    }

    private void SwitchPlayer()
    {
        if (currentPlayerIndex == 1)
        {
            DisablePlayerComponents(currentPlayer); 
            currentPlayer = player2;
            currentPlayerIndex = 2;
        }
        else
        {
            DisablePlayerComponents(currentPlayer);
            currentPlayer = player1;
            currentPlayerIndex = 1;
        }

        // enable other player's components
        EnablePlayerComponents(currentPlayer);
        // update camera follow target
        cameraFollow.SetTarget(currentPlayer);
    }

    private void EnablePlayerComponents(Transform player)
    {
        var movementComponent = player.GetComponent<PlayerMovement>(); 
        if (movementComponent != null)
        {
            movementComponent.enabled = true;
        }
    }

    private void DisablePlayerComponents(Transform player)
    {
        var movementComponent = player.GetComponent<PlayerMovement>(); 
        if (movementComponent != null)
        {
            movementComponent.enabled = false;
        }
    }
}
