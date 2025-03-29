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
        //input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        if (movement.sqrMagnitude > 0.01f) // Only update direction when moving
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }


    void FixedUpdate()
    {
        //movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


}