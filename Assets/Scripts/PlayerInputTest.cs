using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputTest : MonoBehaviour
{
    public bool isGrounded; // Flag to indicate if the player is standing on the ground
    public GameObject projectilePrefab;
    public float platformerMoveSpeed = 1.5f; // Movement speed for platformer control
    public float flyingMoveSpeed = 1f; // Movement speed for flying control
    private bool isFlying = false; // Flag to indicate if character is in flying mode
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Animator animator;
    private Rigidbody2D rb;
    private float gravity = 0.7f;


    public float dashForce = 10f;    // The force applied during dash
    
    public float dashDuration = 0.5f; // The duration of the dash in seconds
    public float dashCooldown = 1.0f;     // The cooldown duration of the dash in seconds

    public float shootCooldown = 1.0f;     // The cooldown duration of the shot in seconds


    private bool isDashing = false;  // Flag to track if the character is currently dashing

    private bool canMove = true;     // Flag to track if the character can move
    private float lastDashTime = -999f;   // The time of the last dash
    private float lastShootTime = -999f;   // The time of the last dash

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is with a platform
        if (collision.collider.CompareTag("Platform"))
        {
            // Player is standing on the platform
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if collision is with a platform
        if (collision.collider.CompareTag("Platform"))
        {
            // Player is no longer standing on the platform
            isGrounded = false;
        }
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
                // Detect input to switch between platformer and flying control
        if (Input.GetKeyDown(KeyCode.E))
        {
            isFlying = !isFlying; // Toggle flying mode
            rb.gravityScale = isFlying ? 0f : gravity; // Enable or disable gravity
                    if (isFlying)
            {
                rb.velocity = Vector2.zero; // Set velocity to zero in flying mode
            }
        }



        // Perform movement based on the current control style
        if (isFlying == false)
        {
            // Platformer control
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 moveDirection = new Vector2(horizontalInput, verticalInput);
            rb.velocity = new Vector2(moveDirection.x * platformerMoveSpeed, rb.velocity.y);
        }
        else
        {
        if(movementInput != Vector2.zero & canMove) {
            rb.MovePosition(rb.position + movementInput * flyingMoveSpeed* Time.fixedDeltaTime);
    }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time - lastDashTime > dashCooldown)
        {
            // Trigger the dash ability
            Dash();
        }

        if (Input.GetKey(KeyCode.Space) && Time.time - lastShootTime > shootCooldown)
        {
            Debug.Log("Shooting");
            animator.SetTrigger("player_attack");
            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.position = gameObject.transform.position;

            lastShootTime = Time.time;
        }

                if (isGrounded && !isFlying)
        {
            // Player is standing on the ground, set isKinematic to true
            rb.isKinematic = true;
            // Player is standing on the ground, freeze rotation and vertical position
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            // Player is not standing on the ground, set isKinematic to false
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
}


    void Dash() {
                // Apply a dash force in the desired direction
        Vector2 dashDirection = movementInput; // Replace with the desired dash direction
        rb.velocity = movementInput * dashForce;
        
        // Set a flag to track the dash duration
        isDashing = true;
        canMove = false;
        StartCoroutine(StopDash());

                // Update the last dash time
        lastDashTime = Time.time;
    }
    IEnumerator StopDash()
    {
        // Wait for the dash duration to complete
        yield return new WaitForSeconds(dashDuration);
        
        // Reset the dash flag and velocity
        isDashing = false;
        canMove = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
}
