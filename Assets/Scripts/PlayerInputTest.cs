using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputTest : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Animator animator;
    private Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if(movementInput != Vector2.zero) {
            // Check for potential collisions
            int count = rb.Cast(
                movementInput, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);

    }
}
    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
}
