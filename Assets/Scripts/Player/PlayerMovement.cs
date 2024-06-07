using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;

    //[HideInInspector]
    public bool isFront = true; // detect which region (front/back) the player is staying at
    private float switchCooldown = 1.0f; // switch CD
    private float lastSwitchTime = 0f;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!enabled || !canMove) return; 

        // get Input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Reverse input if isFront is false
        if (!isFront)
        {
            moveHorizontal = -moveHorizontal;
            moveVertical = -moveVertical;
        }

        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized * moveSpeed;

        // faster falling 
        if (rb.velocity.y < 0)
        {
            // 2.5f means gravity factor
            rb.velocity += Vector3.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
        } 
    }

    void FixedUpdate()
    {
        if (!enabled || !canMove) return; 

        // apply movement
        Vector3 newPosition = rb.position + movement * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    public void SwitchDirection(Vector3 targetPosition, float duration)
    {
        // Only allow switch after CD
        if (Time.time - lastSwitchTime >= switchCooldown)
        {
            isFront = !isFront;
            lastSwitchTime = Time.time;
            StartCoroutine(MoveToPosition(targetPosition, duration));
        }
    }

    // 
    public IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        canMove = false;
        float elapsedTime = 0f;

        Vector3 startingPosition = rb.position;
        while (elapsedTime < duration)
        {
            rb.MovePosition(Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.MovePosition(targetPosition);
        canMove = true;
    }


    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
