using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float boostedJumpForce = 20f; 

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;
    private bool isBoostedJump = false; 
    private bool isLifted = false; 

    //[HideInInspector]
    public bool isFront = true; // detect which region (front/back) the player is staying at
    private float switchCooldown = 1.0f; // switch CD
    private float lastSwitchTime = 0f;
    private bool canMove = true;

    public UnityEvent onJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!enabled) return;

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
        if ((isGrounded || isLifted) && Input.GetButtonDown("Jump"))
        {
            float currentJumpForce = isBoostedJump ? boostedJumpForce : jumpForce;
            rb.useGravity = true; //
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);

            // if lifted, set it false after jumping
            if (isLifted)
            {
                isLifted = false;
                isBoostedJump = false;
                canMove = true; 

                onJump.Invoke();
            }
        }

        if (!isLifted && canMove) // movement is only allowed if not lifted
        {
            movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized * moveSpeed;
        }
        else
        {
            movement = Vector3.zero; // can't move when lifted
        }

        // faster falling 
        if (rb.velocity.y < 0)
        {
            // 2.5f means gravity factor
            rb.velocity += Vector3.up * Physics.gravity.y * (2.5f - 1) * Time.deltaTime;
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

    public void BoostJump(bool isBoosted)
    {
        isBoostedJump = isBoosted;
    }

    public void SetLifted(bool lifted)
    {
        isLifted = lifted;
        rb.useGravity = !lifted; 
        if (lifted)
        {
            canMove = false; // restrict movement when lifted
        }
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
