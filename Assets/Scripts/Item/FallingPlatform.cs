using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 0.2f; 
    public float destroyDelay = 0.1f;

    private Rigidbody rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            Invoke("StartFalling", fallDelay);
        }
    }

    void StartFalling()
    {
        rb.isKinematic = false; 
        Destroy(gameObject, destroyDelay);
    }
}
