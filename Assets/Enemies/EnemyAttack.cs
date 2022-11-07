using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private PlayerHealth target;
    [SerializeField] private int meleeDamage = 20;

    void Start() {
        target = FindObjectOfType<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        target = other.transform.GetComponent<PlayerHealth>();
        MeleeAttack();
    }

    private void MeleeAttack() {
        if (target == null) { return; }
        target.TakeDamage(meleeDamage, transform.position);
    }
}
