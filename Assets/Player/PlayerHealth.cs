using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isAlive = true;

    private bool invincible = false;
    private HealthBar healthBar;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CameraShake CameraShake;

    [SerializeField] AudioSource damageSound;
    [SerializeField] AudioSource deathSound;

    void Start() {
        currentHealth = maxHealth;
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        if (healthBar) {healthBar.SetMaxHealth(maxHealth); }
        CameraShake = Camera.main.GetComponent<CameraShake>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Vector2 location) {
        if (invincible) { return; } // Ignore if player is invincible (i.e. while dashing)
        damageSound.Play();
        KnockBack(location);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
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
        Destroy(GetComponentInChildren<SpriteRenderer>());
        Destroy(rb);
    }

    public bool Invincible {
        get { return invincible; }
        set { invincible = value; }
    }

    private void KnockBack(Vector3 location) {
        Vector2 destination = (location - transform.position) * -1;
        Debug.Log("Position: " + transform.position + " Location: " + location + " Destination: " + destination);
        // Apply force to rigidbody
    }
}
