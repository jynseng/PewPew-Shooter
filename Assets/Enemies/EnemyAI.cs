using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{   
    [Tooltip("How far to scan for targets")]
    [SerializeField] private float scanRadius = 5f;

    [Tooltip("Distance to target to start chasing")]
    [SerializeField] private float chaseRadius = 50f;

    [Tooltip("Max distance to random pathfinding destination")]
    [SerializeField] private float roamRadius = 5f;

    [SerializeField] private float moveSpeed = 1f;

    [Tooltip("Seconds bewteen shots")]
    [SerializeField] private float shootCooldown = 3f;

    [Tooltip("Whether or not can shoot, for testing/debugging")]
    [SerializeField] private bool canShoot = true;

    [SerializeField] AudioSource shootSound = null;
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] EnemyHealth health = null;

    private bool targeting = false;
    private float distanceToTarget = Mathf.Infinity;
    private float distanceToRandomDest;
    private float bulletForce = 12f;
    private float shootCooldownCounter = 0;

    Vector2 randomDest;
    Vector2 currentPos;
    Transform target = null;
    PlayerHealth playerHealth;

    private void Start() {
        playerHealth = FindObjectOfType<PlayerHealth>();
        target = playerHealth.transform;
        targeting = false;

        currentPos = new Vector2(transform.position.x, transform.position.y); // Convert current position to 2D vector
        PickNewRandomDest();
    }

    private void Update() {
        if (playerHealth.isAlive) {
            distanceToTarget = Vector2.Distance(target.position, transform.position);
        } else {
            distanceToTarget = Mathf.Infinity;
            targeting = false;
        }
        distanceToRandomDest = Vector2.Distance(randomDest, transform.position);

        // If player out of reach while targeting, stop chasing and start roaming again
        if (targeting && distanceToTarget > chaseRadius) {
            targeting = false;
            PickNewRandomDest();
        }  

        // If player target comes within scan radius, start chasing
        if (targeting || distanceToTarget <= scanRadius) { 
            targeting = true; 
            ChasePlayer();
        } else { 
            transform.position = Vector2.MoveTowards(transform.position, randomDest, moveSpeed * Time.deltaTime); // If not within radius, just roam
            if (distanceToRandomDest <= 0.4f) {
                PickNewRandomDest();
            }
        }

        if (shootCooldownCounter > 0) {
            shootCooldownCounter -= Time.deltaTime; 
        }
    }

    private void ChasePlayer() {
        if (distanceToTarget >= 1) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        if (shootCooldownCounter <= 0) {
            Shoot();
            shootCooldownCounter = shootCooldown; // Reset shoot cooldown
        }
    }

    private void Shoot() {
        if (health.currentHealth <= 0 || !playerHealth.isAlive || !canShoot) { return; } // If this enemy is dead, don't shoot
        shootSound.Play();
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null) {return;}
        rb.AddForce((target.position-firePoint.position).normalized * bulletForce, ForceMode2D.Impulse);
    }

    private void PickNewRandomDest() {
        randomDest = currentPos + Random.insideUnitCircle.normalized * roamRadius;
    }
}
