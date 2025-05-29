using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enum for player state machine
public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Falling,
    Dash,
    Roll
    // Add other states as needed
}

public class PlayerMovement : MonoBehaviour
{
public Animator anim;
public bool wall;
    [Header("Camera")]
    [SerializeField] private Camera Camera;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private bool onWall;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpCutMultiplier = 0.25f;
    [SerializeField] private int wallJumpY;
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
    private bool wallJumpCD = false;
    private PlayerAbility ability;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ability = GetComponent<PlayerAbility>();
        // Initialize the state actions dictionary
        InitializeStateActions();
        StateCheck("Idle");
        wallJumpY = 600;
    }

    private void InitializeStateActions()
    {
       
        
        stateActions = new Dictionary<string, Action>
        {
            // Idle state setup
            { "Idle", () => {
                playerState = PlayerState.Idle;
                anim.Play("idle");
                // Any Idle-specific animation goes here
            }},
            
            // Running state setup
            { "Moving", () => {
                playerState = PlayerState.Moving;
                anim.Play("run");
                // Any Running-specific animation goes here
            }},
            
            // Jumping state setup
            { "Jumping", () => {
                playerState = PlayerState.Jumping;
                anim.Play("jump");
                // Any Jumping-specific animation goes here
            }},
            
            // Falling state setup
            { "Falling", () => {
                playerState = PlayerState.Falling;
                if(onWall){
                    anim.Play("wall");
                }
                // Any Falling-specific logic goes here
            }},

            // Idle state setup
            { "Dash", () => {
                playerState = PlayerState.Dash;
                anim.Play("roll");
                // Any Dash-specific animation goes here
            }},

             // Idle state setup
            { "Roll", () => {
                playerState = PlayerState.Roll;
                anim.Play("roll");
                // Any Dash-specific animation goes here
            }},
            
            // Add more states as needed
        };
    }

    void Update()
    {
       
        moveInput = Input.GetAxis("Horizontal");

        Move();

        HandleGroundDetection();

        Jump();

        JumpPhysics();

        // Update camera position to follow player
        Camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    private void HandleGroundDetection()
    {
        if (IsGrounded())
        {
          
          
            coyoteTimeCounter = coyoteTime;

            // When landing, transition to Running based on horizontal input
            if ((playerState == PlayerState.Jumping || playerState == PlayerState.Falling) && rb.velocity.y <= 0)
            {
               
                if (Mathf.Abs(moveInput) > 0.1f)
                {
                    if (!ability.iFrame)
                        StateCheck("Moving");
                    else
                        StateCheck("Roll");
                }
                    
            }
           
        }

        else
        {
            coyoteTimeCounter -= Time.deltaTime;

            // If we're not jumping and not grounded, we must be falling
            if (playerState != PlayerState.Jumping && playerState != PlayerState.Falling)
            {
                // No fall penalty when wall jumping
                StateCheck("Falling");
                float freeFall = onWall ? moveSpeed : fallSpeed;
                rb.velocity = new Vector2(freeFall, rb.velocity.y);
            }

        }
    }

    private void Move()
    {
        if(playerState != PlayerState.Dash) {
            // Calculate target speed
            moveInput *= moveSpeed;
            rb.velocity = new Vector2(moveInput, rb.velocity.y);
        }
        

        if (rb.velocity.x == 0f && rb.velocity.y == 0f)
            StateCheck("Idle");

        // Update running state, player can only move if grounded
        if (IsGrounded())
        {

            if (Mathf.Abs(rb.velocity.x) > 0.1f && playerState != PlayerState.Moving)
            {
                if(!ability.iFrame)
                    StateCheck("Moving");
                else
                    StateCheck("Roll");
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
            Vector2 jumpVector = new Vector2(rb.velocity.x, jumpForce);
            // Apply jump velocity
            rb.AddForce(jumpVector, ForceMode2D.Impulse);
            StateCheck("Jumping");

            // Reset coyote timer after jump
            coyoteTimeCounter = 0;
        }

        // Only works if player is falling down wall, not grounded, and not on cooldown
        if (Input.GetKeyDown(KeyCode.Space) && onWall
            && (playerState == PlayerState.Falling) && !onGround
            && !wallJumpCD)
        {
            Debug.Log("Wall jump");
      
            rb.drag = 0;
            // Instantaneous force needs a much higher number (hundreds) to scale properly
            Vector2 wallJump = new Vector2(rb.velocity.x, wallJumpY);
            rb.AddForce(wallJump);

            StateCheck("Jumping");
            StartCoroutine(WallJumpCooldown());

        }
    }

    private void JumpPhysics()
    {
        // Cut jump short when button is released (variable jump height)
        if (rb.velocity.y > 0 && Input.GetKeyUp(KeyCode.Space) && !onWall)
        {
            StateCheck("Jumping");
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        // Check vertical velocity while in air
        if (rb.velocity.y < 0 && playerState != PlayerState.Falling)
        {
            StateCheck("Falling");
        }

        if(rb.velocity.y > 0 && playerState != PlayerState.Jumping)
        {
            StateCheck("Jumping");
        }

    }

    public bool IsGrounded()
    {
        // Use a raycast to check if player is touching the ground
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return onGround;
    }

    public void StateCheck(string newState)
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

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void IsOnWall(bool madeContact)
    {
        if(madeContact){
        wall=true;
        }
        else{
          wall=false;
        }
        onWall = madeContact;
    }

    IEnumerator WallJumpCooldown()
    {
       
        float cooldown = 2f;
        wallJumpCD = true;
        while (cooldown >  0) {
            cooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        wallJumpCD = false;
    }
}
