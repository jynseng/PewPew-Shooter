using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb; 
    public float moveSpeed = 5f;
    public bool invincible = false;
    public StaminaBar StaminaBar;
    public float stamina;
    Shooting shooting = null;    
    
    [SerializeField] private float dashSpeed = 35f;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private float maxStamina = 10f;
    [SerializeField] private float dashWindow_default = 0.4f; // Length of chain dash window in seconds
    [SerializeField] private Transform dashBar;
    [SerializeField] AudioSource dashSound;
    [SerializeField] AudioSource chainDashSound;
    [SerializeField] AudioSource chainDashSound_2;

    private float dashWindow;
    private float dashCost = 3f; // Stamina cost of dash
    private float staminaRegenRate = 1.5f; // Stamina points regenerated per second
    private float dashTimer = -1f; // Timer that starts at dash start
    private bool withinWindow = false; // Was "dash" key pressed within chain-dash window?
    private bool dashAttempted = false; // Has player inputted "dash" during current dash? 
    private int chainDashCount = 0; // How many dashes have been chained this dash?

    private Vector2 moveInput;
    private Vector2 dashDirection;
    private float activeMoveSpeed;
    private float dashCounter; // Active dashing counter
    private CameraShake CameraShake;
    
    void Start() {
        activeMoveSpeed = moveSpeed;
        dashBar.localScale = new Vector3(0,0,0);
        dashWindow = dashWindow_default;
        stamina = maxStamina;
        if (StaminaBar) {StaminaBar.SetMaxStamina(maxStamina);}
        shooting = GetComponent<Shooting>();
        CameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update() {
        // Player movement
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Dash
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (moveInput.magnitude > 0) {
                dashDirection = moveInput;
            } else {
                dashDirection = shooting.FaceDirection();
            }
            DashHandler();
        }   

        // If currently dashing, decrement dash counter
        if (dashCounter > 0) {
            dashCounter -= Time.deltaTime; 
            Vector3 start = new Vector3(1,1,1);
            Vector3 end = new Vector3 (0,0,0);
            dashBar.localScale = Vector3.Lerp(start, end, (dashTimer/dashLength+(dashWindow/2))); // Scale dashBar with dashCounter for testing purposes.
            if (dashCounter <= 0) { // If end of dash, then...
                activeMoveSpeed = moveSpeed; // Reset move speed to normal
                invincible = false;
            }
        }

        if (dashTimer >= 0) { // Dash started
            dashTimer += Time.deltaTime; 
            if (dashTimer >= dashLength-(dashWindow/2) && dashTimer < dashLength+(dashWindow/2)) { // Check if within dash window 
                withinWindow = true;
                dashBar.GetComponent<SpriteRenderer>().color = Color.blue;
            } else if (dashTimer < dashLength) { // Mid-dash before window
                dashBar.GetComponent<SpriteRenderer>().color = Color.white;
            } else if (dashTimer < dashLength+dashCooldown) { // After dash, after window, before cooldown
                chainDashCount = 0;
                dashBar.localScale = new Vector3(0,0,0); // Hide the dashBar visual indicator
                dashBar.GetComponent<SpriteRenderer>().color = Color.white;
                withinWindow = false;
            } else { // Fully cooled down
                dashTimer = -1f;
                dashAttempted = false;
                dashWindow = dashWindow_default; // Reset dashWindow to default, in case player was chain-dashing
            }
        }

        // Regenerate stamina over time, if not currently dashing
        if (stamina < maxStamina && dashCounter <= 0 && dashTimer < 0) {
            stamina += Time.deltaTime * staminaRegenRate;
            StaminaBar.SetStamina(stamina);
        }
    }

    void FixedUpdate() {
        if (dashCounter <= 0) { 
            rb.MovePosition(rb.position + moveInput.normalized * activeMoveSpeed * Time.fixedDeltaTime); // Handle actual movement independent of framerate
        } else {
            rb.MovePosition(rb.position + dashDirection.normalized * activeMoveSpeed * Time.fixedDeltaTime); // Handle movement while dashing
        }
    }
    
    // Check if player can dash
    private void DashHandler() {
        if (dashTimer == -1 && stamina > dashCost) { // Normal dash, must have enough stamina
            stamina -= dashCost; // Dash costs player stamina
            StaminaBar.SetStamina(stamina);
            Dash();
            return;
        }

        if (withinWindow && !dashAttempted && chainDashCount < 4) { // Chain-dash, no stamina req'd
            chainDashCount++;
            chainDashSound.volume = chainDashCount*0.2f + 0.1f; // SFX gets louder each successive dash
            chainDashSound.Play();
            if (chainDashCount == 4) {
                chainDashSound_2.Play();
            }
            dashWindow *= 0.5f; // Cut window in half (player must have more precise timing each successive dash)
            Dash();
            withinWindow = false; 
            return;
        }

        dashAttempted = true;
    }

    private void Dash() {
        dashTimer = 0;
        dashSound.Play();
        dashBar.localScale = new Vector3(1,1,1);
        activeMoveSpeed = dashSpeed;
        dashCounter = dashLength;
        invincible = true; // Become invincible while dashing
        CameraShake.StartShake();       
    }
}
