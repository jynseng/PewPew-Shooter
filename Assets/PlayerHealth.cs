using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    [SerializeField] AudioSource damageSound = null;

    void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update() {

    }

    public void TakeDamage(int damage) {
        damageSound.Play();
        
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth == 0) {
            Debug.Log("Game Over");
        }
    }
}
