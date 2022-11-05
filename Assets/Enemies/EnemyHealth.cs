using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 50;
    [SerializeField] AudioSource damageSound = null;
    [SerializeField] AudioSource deathSound = null;

    public HealthBar healthBar;
    public int currentHealth;
    private Canvas canvas;

    void Start() {
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        canvas = GetComponentInChildren(typeof(Canvas)) as Canvas;
        canvas.enabled = false;
    }

    public bool TakeDamage(int damage) {
        canvas.enabled = true;
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        damageSound.Play();
        if (currentHealth <= 0) {Die();}
        return true;
    }

    private void Die() {
        deathSound.Play(); // Play SFX
        GetComponentInChildren<Canvas>().enabled = false;
        Destroy(gameObject.GetComponentInChildren<SpriteRenderer>());
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        // GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2f);
    }
}
