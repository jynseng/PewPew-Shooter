using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float healthPoints = 50f;
    [SerializeField] AudioSource damageSound = null;

    public bool TakeDamage(float damage) {
        
        // damageSound.Play();
        healthPoints -= Mathf.Abs(damage);
        if (healthPoints <= 0)
        {
            Die();
        }
        return true;

    }

    private void Die() {
        Debug.Log("I am ded");
        Destroy(gameObject);
        // GetComponent<Animator>().SetTrigger("Die");
    }
}
