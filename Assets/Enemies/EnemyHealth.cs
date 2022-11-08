using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 50;
    [SerializeField] AudioSource damageSound = null;
    [SerializeField] AudioSource deathSound = null;
    private HealthBar healthBar;
    public int currentHealth;

    private Canvas canvas;
    private SpriteFlash spriteFlash;

    void Start() {
        currentHealth = maxHealth;
        canvas = GetComponentInChildren(typeof(Canvas)) as Canvas;
        canvas.enabled = false;
        healthBar = canvas.GetComponentInChildren(typeof(HealthBar)) as HealthBar;
        healthBar.SetMaxHealth(currentHealth);
        spriteFlash = GetComponent<SpriteFlash>();
    }

    public bool TakeDamage(int damage, Vector2 location) {
        // Stop moving
        canvas.enabled = true;
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        damageSound.Play();
        spriteFlash.Flash();
        if (currentHealth <= 0) { Die(); }
        return true;
    }

    private void Die() {
        deathSound.Play(); // Play SFX
        GetComponentInChildren<Canvas>().enabled = false;
        foreach(var sprite in GetComponentsInChildren<SpriteRenderer>()) {
            sprite.enabled = false;
        }
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        // GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2f);
    }
}
