using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb; 
    public float moveSpeed = 5f;
    public bool invincible = false;
    
    [SerializeField] private float dashSpeed = 32f;
    [SerializeField] private float dashLength = 0.2f;
    [SerializeField] private float dashCooldown = 0.3f;
    [SerializeField] private Transform dashBar;

    // Experimental dash timing mechanic:
    [SerializeField] private float defaultIntervalStart = 0.6f; // Minimum time after dashing when player can dash again (percent of dashLength)
    [SerializeField] private float maxStamina = 10f;
    public StaminaBar staminaBar;
    public float stamina; // Current player stamina
    private float dashCost = 3f; // Stamina cost of dash
    private float staminaRegenRate = 1.5f; // Stamina points regenerated per second
    private float intervalStart;
    //private float intervalEnd; // Amount of time after dashing when player will get maximum dash (end of timing interval)
    //private float maxWindow = .5f; // Window of time to get maximum dash, starts at intervalEnd
    //private float fullDashTime = 3f; // Full amount of time from dash start to end to cooldown ready again (to calculate dash timing boost)
    //private float fullDashCounter;

    private Vector2 moveInput;
    private Vector2 dashDirection; 
    private float activeMoveSpeed;
    private float dashCounter; // Active dashing counter
    private float dashCoolCounter; // Cooldown counter after performing a dash
    

    void Start() {
        activeMoveSpeed = moveSpeed;
        dashCoolCounter = 0;
        dashBar.localScale = new Vector3(0,0,0);
        intervalStart = dashLength*defaultIntervalStart;
        stamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    void Update() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space)) {
            dashDirection = moveInput;
            Dash();
        }   

        // If currently dashing, decrement dash counter
        if (dashCounter > 0) {
            dashCounter -= Time.deltaTime; 
            Vector3 start = new Vector3(1,1,1);
            Vector3 end = new Vector3 (0,0,0);
            dashBar.localScale = Vector3.Lerp(start, end, (1-dashCounter/dashLength)); // Scale dashBar with dashCounter for testing purposes.
            if (dashCounter <= intervalStart) { // Turn indicator blue when interval start 
                dashBar.GetComponent<SpriteRenderer>().color = Color.blue;
            } else {
                dashBar.GetComponent<SpriteRenderer>().color = Color.white;
            }

            if (dashCounter <= 0) { // If end of dash, then...
                activeMoveSpeed = moveSpeed; // Reset move speed to normal
                dashCoolCounter = dashCooldown; // Start cooldown timer
                invincible = false;
                dashBar.localScale = new Vector3(0,0,0); // Hide the dashBar visual indicator
                intervalStart = dashLength*defaultIntervalStart; // Reset intervalStart to default, in case player was chain-dashing
            }
        }

        // If dash cooldown counting down, decrement counter each frame
        if (dashCoolCounter > 0) {
            dashCoolCounter -= Time.deltaTime;
        }

        // Regenerate stamina over time, if not currently dashing
        if (stamina < maxStamina && dashCounter <= 0 && dashCoolCounter < 0.1) {
            stamina += Time.deltaTime * staminaRegenRate;
            staminaBar.SetStamina(stamina);
        }
    }

    void FixedUpdate() {
        if (dashCounter <= 0) { 
            rb.MovePosition(rb.position + moveInput.normalized * activeMoveSpeed * Time.fixedDeltaTime); // Handle actual movement independent of framerate
        } else {
            rb.MovePosition(rb.position + dashDirection.normalized * activeMoveSpeed * Time.fixedDeltaTime); // Handle movement while dashing
        }
    }
    
    private void Dash() {
        if (dashCoolCounter <= 0 && stamina >= dashCost) { // If dash has "cooled down"...
            if (dashCounter > 0) { // If player is currently dashing...
                if (dashCounter <= intervalStart) { // Chain dash executed
                    intervalStart *= 0.8f; // Shrink interval window after every chain dash
                    stamina += dashCost;
                }  else if (dashCounter >= intervalStart){ // Chain dash failed
                    // Disable dash button until set timer to prevent button spamming
                    return;
                }
            }
            stamina -= dashCost; // Dash costs player stamina
            staminaBar.SetStamina(stamina);
            dashBar.localScale = new Vector3(1,1,1);
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            invincible = true; // Become invincible while dashing       
        }
    }
}
