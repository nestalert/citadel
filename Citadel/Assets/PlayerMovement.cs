using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        // Reset movement at the beginning of the frame
        movement = Vector2.zero;

        // Input
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            movement.x = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            movement.x = -1f;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            movement.y = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            movement.y = -1f;
        }

        // Normalize diagonal movement to prevent faster speed
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        // Update Animator parameters only when there is input
        if (movement.sqrMagnitude > 0)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("IsMoving", 1f); // Set IsMoving to true when there's movement
        }
        else
        {
            animator.SetFloat("IsMoving", 0f); // Set IsMoving to false when no movement
        }

        // You might not need a separate "Speed" float if you're using IsMoving
        // animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}