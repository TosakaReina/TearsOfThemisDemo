using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // player position
    public Vector3 offset;
    public float followSpeed = 5f;

    private PlayerMovement targetMovement;

    private void Start()
    {
        targetMovement = target.GetComponent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (target != null && targetMovement != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            //transform.LookAt(target);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetMovement = target.GetComponent<PlayerMovement>();
    }
}
