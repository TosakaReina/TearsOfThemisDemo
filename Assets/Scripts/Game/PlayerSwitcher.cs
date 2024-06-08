using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public CameraSwitchController cameraSwitchController;

    private CameraFollow cameraFollow;
    private Transform currentPlayer; // current player's transform
    private int currentPlayerIndex = 1;

    void Start()
    {
        // initialize player 1 as current player
        currentPlayer = player1;
        cameraFollow = cameraSwitchController.currentCamera.GetComponent<CameraFollow>();
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
        // Disable current player components
        DisablePlayerComponents(currentPlayer);

        // Switch current player
        if (currentPlayerIndex == 1)
        {
            currentPlayer = player2;
            currentPlayerIndex = 2;
        }
        else
        {
            currentPlayer = player1;
            currentPlayerIndex = 1;
        }

        // Enable new current player components
        EnablePlayerComponents(currentPlayer);

        // Update camera follow target
        UpdateCameraFollow();
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

    private void UpdateCameraFollow()
    {
        // Detect whether both players are in the same region (front or back)
        if (player1.GetComponent<PlayerMovement>().isFront != player2.GetComponent<PlayerMovement>().isFront)
        {
            // Switch camera based on the current player's region
            cameraSwitchController.ToggleCamera(currentPlayer);
        }

        // Update camera follow component with the new target
        cameraFollow = cameraSwitchController.currentCamera.GetComponent<CameraFollow>();
        cameraFollow.SetTarget(currentPlayer);
    }

}
