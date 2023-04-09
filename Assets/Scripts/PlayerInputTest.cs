using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputTest : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    private Rigidbody2D rb;

    public Animator animator;

    public float dashForce = 10f;    // The force applied during dash
    
    public float dashDuration = 0.5f; // The duration of the dash in seconds
    public float dashCooldown = 1.0f;     // The cooldown duration of the dash in seconds

    public float shootCooldown = 1.0f;     // The cooldown duration of the shot in seconds


    private bool isDashing = false;  // Flag to track if the character is currently dashing

    private bool canMove = true;     // Flag to track if the character can move
    private float lastDashTime = -999f;   // The time of the last dash
    private float lastShootTime = -999f;   // The time of the last dash



    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if(movementInput != Vector2.zero & canMove) {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
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
