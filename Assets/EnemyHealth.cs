using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float healthPoints = 50f;
    [SerializeField] AudioSource damageSound = null;
    [SerializeField] AudioSource deathSound = null;

    public bool TakeDamage(float damage) {
        
        damageSound.Play();
        healthPoints -= Mathf.Abs(damage);
        if (healthPoints <= 0)
        {
            Die();
        }
        return true;

    }

    private void Die() {
        deathSound.Play(); // Play SFX
        Destroy(gameObject.GetComponent<SpriteRenderer>());
        // GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2f);
    }
}
