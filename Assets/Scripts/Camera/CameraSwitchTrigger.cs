using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchTrigger : MonoBehaviour
{
    public CameraSwitchController cameraSwitchController;
    public Transform frontTargetPosition; // target pos if player is in front region
    public Transform backTargetPosition; // target pos if player is back region
    public float moveDuration = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            Vector3 targetPosition = playerMovement.isFront ? frontTargetPosition.position : backTargetPosition.position;

            cameraSwitchController.ToggleCamera(other.transform);
            playerMovement.SwitchDirection(targetPosition, moveDuration);
        }
    }
}
