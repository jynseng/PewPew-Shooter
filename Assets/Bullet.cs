using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] float damage = 10f;

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        EnemyHealth target = collision.gameObject.GetComponent<EnemyHealth>();
        if (target == null) { return; }
        target.TakeDamage(damage);

        Destroy(effect, 5f);
        Destroy(gameObject);
    }

    
}
