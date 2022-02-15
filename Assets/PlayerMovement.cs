using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb; 
    public float moveSpeed = 5f;
    
    [SerializeField] private float dashSpeed = 26f;
    [SerializeField] private float dashLength = 0.15f;
    [SerializeField] private float dashCooldown = .3f;

    private Vector2 moveInput; 
    private float activeMoveSpeed;
    private float dashCounter;
    private float dashCoolCounter;

    void Start() {
        activeMoveSpeed = moveSpeed;
    }

    void Update() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space)) {
            Dash();
        }

        // If currently dashing, decrement dash counter
        if (dashCounter > 0) {
            dashCounter -= Time.deltaTime; 

            if (dashCounter <= 0) {
                activeMoveSpeed = moveSpeed; // Reset move speed to normal
                dashCoolCounter = dashCooldown; // Start cooldown timer
            }
        }

        // If dash cooldown counting down, decrement counter each frame
        if (dashCoolCounter > 0) {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + moveInput * activeMoveSpeed * Time.fixedDeltaTime); // Handle actual movement independent of time

    }
    
    private void Dash() {
        if (dashCoolCounter <=0 && dashCounter <=0) { // Can only dash if cooldown ready and not currently dashing
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
        }
    }
}
