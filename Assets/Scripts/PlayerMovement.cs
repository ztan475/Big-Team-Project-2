using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Enum for player state machine
    public enum PlayerState
    {
        Idle,
        Moving,
        Jumping,
        Falling
        // Add other states as needed
    }
    [Header("Camera")]
    [SerializeField] private Camera Camera;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PlayerState playerState;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpCutMultiplier = 0.25f;
    [SerializeField] private float fallSpeed;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool onGround;

    private Rigidbody2D rb;
    private float moveInput;
    private float coyoteTimeCounter;
    private Dictionary<string, Action> stateActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fallSpeed = (moveSpeed / 4);
        // Initialize the state actions dictionary
        InitializeStateActions();
        StateCheck("Idle");
    }

    private void InitializeStateActions()
    {
        stateActions = new Dictionary<string, Action>
        {
            // Idle state setup
            { "Idle", () => {
                playerState = PlayerState.Idle;
                // Any Idle-specific animation goes here
                Debug.Log("Player entered Idle state");
            }},
            
            // Running state setup
            { "Moving", () => {
                playerState = PlayerState.Moving;
                // Any Running-specific animation goes here
                Debug.Log("Player entered Moving state");
            }},
            
            // Jumping state setup
            { "Jumping", () => {
                playerState = PlayerState.Jumping;
                // Any Jumping-specific animation goes here
                Debug.Log("Player entered Jumping state");
            }},
            
            // Falling state setup
            { "Falling", () => {
                playerState = PlayerState.Falling;
                // Any Falling-specific logic goes here
                Debug.Log("Player entered Falling state");
            }},
            
            // Add more states as needed
        };
    }

    private void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        HandleGroundDetection();

        Jump();

        JumpPhysics();

        // Update camera position to follow player
        Camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    private void FixedUpdate()
    {
        // Handle horizontal movement
        Move();
    }

    private void HandleGroundDetection()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;

            // When landing, transition to Idle or Running based on horizontal input
            if ((playerState == PlayerState.Jumping || playerState == PlayerState.Falling) && rb.velocity.y <= 0)
            {
                if (Mathf.Abs(moveInput) > 0.1f)
                    StateCheck("Moving");
                else
                    StateCheck("Idle");
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;

            // If we're not jumping and not grounded, we must be falling
            if (playerState != PlayerState.Jumping && playerState != PlayerState.Falling)
            {
                StateCheck("Falling");
                rb.velocity = new Vector2(fallSpeed, rb.velocity.y);
            }

        }
    }

    private void Move()
    {
        // Calculate target speed
        moveInput *= moveSpeed;
        rb.velocity = new Vector2(moveInput, rb.velocity.y);

        // Update running state, player can only move if grounded
        if (IsGrounded())
        {

            if (Mathf.Abs(rb.velocity.x) > 0.1f && playerState != PlayerState.Moving)
            {
                StateCheck("Moving");
            }
            else if (Mathf.Abs(rb.velocity.x) <= 0.1f && playerState != PlayerState.Idle)
            {
                StateCheck("Idle");
            }
        }
    }

    private void Jump()
    {
        // Check if the spacebar is pressed and ensure jump conditions are met
        if (Input.GetKeyDown(KeyCode.Space) && 
            coyoteTimeCounter > 0 &&
            (playerState != PlayerState.Jumping))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump velocity
            StateCheck("Jumping");

            // Reset coyote timer after jump
            coyoteTimeCounter = 0;
        }
    }

    private void JumpPhysics()
    {
        // Cut jump short when button is released (variable jump height)
        if (rb.velocity.y > 0 && Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        // Fall faster than jump for better feel
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        // Use a raycast to check if player is touching the ground
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return onGround;
    }

    private void StateCheck(string newState)
    {
        // Check if the state exists in our dictionary
        if (stateActions.TryGetValue(newState, out Action stateAction))
        {
            // Execute the state change action
            stateAction();
        }
        else
        {
            Debug.LogWarning("Attempted to change to unknown state: " + newState);
        }
    }

    // Draw ground check in the editor for debugging
    // Will not work if the groundCheck object is not assigned in inpector
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}
