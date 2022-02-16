using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] int damage = 10;


    void OnEnable() {
        Invoke("SelfDestruct", 4);
    }

    void SelfDestruct() {
        Destroy(gameObject);
    }   

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);

        EnemyHealth target = collision.gameObject.GetComponent<EnemyHealth>();
        if (target != null) {
            target.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    
}
