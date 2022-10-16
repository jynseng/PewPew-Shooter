using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    PlayerMovement movement;

    [SerializeField] AudioSource damageSound;
    [SerializeField] AudioSource deathSound;

    void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        movement = gameObject.GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int damage) {
        if (movement.invincible) {return;} // If player is invincible (i.e. while dashing, as tracked in "PlayerMovement"), return
        
        damageSound.Play();
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        // Create separate gameObject to play Death SFX, lets Player object self-destruct immediately
        GameObject deathEffects = new GameObject("deathEffects");
        AudioSource audioSource = deathEffects.AddComponent<AudioSource>();
        audioSource.clip = deathSound.clip;
        audioSource.Play();
        Destroy(audioSource, 2f);
        
        Destroy(gameObject);
    }
}
