using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] int damage = 10;
    public bool isPlayerBullet = true;

    void OnEnable() {
        Invoke("SelfDestruct", 4);
    }

    void SelfDestruct() {
        Destroy(gameObject);
    }   

    void OnTriggerEnter2D(Collider2D col) {
        if ((!isPlayerBullet && col.tag == "Enemy") || (isPlayerBullet && col.tag == "Player") || (col.tag == "Player" && col.GetComponent<PlayerMovement>().invincible)
         || col.tag == "Projectile") {return;} // Don't collide with self, other bullets, or invincible player (i.e. while dashing)

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity); // Explosion effect
        Destroy(effect, 2f);

        EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
        PlayerHealth playerHealth = col.gameObject.GetComponent<PlayerHealth>();
        if (enemyHealth != null) {
            enemyHealth.TakeDamage(damage);
        } else if (playerHealth != null) {
            playerHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    
}
