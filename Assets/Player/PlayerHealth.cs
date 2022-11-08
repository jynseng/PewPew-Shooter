using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isAlive = true;

    private bool isInvincible = false;
    private HealthBar healthBar;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private SpriteFlash spriteFlash;
    [SerializeField] CameraShake CameraShake;
    [Tooltip("Plays when taking damage")]
    [SerializeField] AudioSource damageSound;
    [Tooltip("Plays when dying")]
    [SerializeField] AudioSource deathSound;

    void Start() {
        currentHealth = maxHealth;
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        if (healthBar) {healthBar.SetMaxHealth(maxHealth); }
        CameraShake = Camera.main.GetComponent<CameraShake>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteFlash = GetComponent<SpriteFlash>();
    }

    public void AddHealth(int health) {
        Debug.Log("Adding " + health + " health");
        currentHealth += health;
    }

    public void TakeDamage(int damage, Vector2 location) {
        if (isInvincible) { return; } // Ignore if player is invincible (i.e. while dashing)
        damageSound.Play();
        StartCoroutine(KnockBackCoroutine(damage, location));
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        spriteFlash.Flash();
        CameraShake.StartShake();
        if (currentHealth <= 0) { Die(); }
    }

    private void Die() {
        isAlive = false;
        GameObject deathEffects = new GameObject("deathEffects"); // Create separate gameObject to play Death SFX, lets Player object self-destruct immediately
        AudioSource audioSource = deathEffects.AddComponent<AudioSource>();
        audioSource.clip = deathSound.clip;
        audioSource.Play();
        Destroy(audioSource, 2f);
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>()) {
            sprite.enabled = false;
        }
        Destroy(GetComponentInChildren<BoxCollider2D>());
        Destroy(rb, 0.5f);
    }

    public bool IsInvincible {
        get { return isInvincible; }
        set { isInvincible = value; }
    }

    private IEnumerator KnockBackCoroutine(int damage, Vector3 location) {
        playerMovement.CanMove = false;
        float thrust = damage * 5f;
        Vector3 delta = (transform.position-location).normalized;
        Vector2 force = delta * thrust;
        if (rb != null) { 
            rb.AddForce(force, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.1f);
        if (rb != null) {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        playerMovement.CanMove = true;
    }

    /* Knock player back based on how much damage taken. (Kinematic only)
    private IEnumerator KnockBackCoroutine(int damage, Vector3 location) {
        playerMovement.CanMove = false; // Momentarily disable player movement controller to apply velocity
        float thrust = damage * 1.5f;
        Vector3 delta = (transform.position-location).normalized;
        Vector3 force = delta * thrust;
        rb.velocity = new Vector2(force.x, force.y);
        yield return new WaitForSeconds(0.1f);
        playerMovement.CanMove = true;
    } */
}
