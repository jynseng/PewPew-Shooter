using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //EnemyHealth health = null;
    
    [SerializeField] float scanRadius = 5f;
    [SerializeField] float chaseRadius = 50f;
    [SerializeField] float roamRadius = 3f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float shootCooldown = 3f;

    [SerializeField] AudioSource shootSound = null;
    [SerializeField] GameObject enemyBulletPrefab;
    public Transform firePoint;

    bool targeting = false;
    float distanceToTarget = Mathf.Infinity;
    float distanceToRandomDest;
    float bulletForce = 12f;
    float shootCooldownCounter = 0;

    Vector2 randomDest;
    Vector2 currentPos;
    Transform target = null;

    void Start() {
        //health = GetComponent<EnemyHealth>();
        target = FindObjectOfType<PlayerHealth>().transform;
        targeting = false;

        currentPos = new Vector2(transform.position.x, transform.position.y); // Convert current position to 2D vector
        PickNewRandomDest();
    }

    void Update() {
        distanceToTarget = Vector2.Distance(target.position, transform.position);
        distanceToRandomDest = Vector2.Distance(randomDest, transform.position);

        // If player out of reach while targeting, stop chasing and start roaming
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
            if (distanceToRandomDest <= .5) {
                PickNewRandomDest();
            }
        }

        if (shootCooldownCounter > 0) {
            shootCooldownCounter -= Time.deltaTime; 
        }
    }

    void ChasePlayer() {
        if (distanceToTarget >= 1) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        if (shootCooldownCounter <= 0) {
            Shoot();
            shootCooldownCounter = shootCooldown; // Reset shoot cooldown
        }
    }

    void Shoot() {
        shootSound.Play();
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null) {return;}
        rb.AddForce((target.position-firePoint.position).normalized * bulletForce, ForceMode2D.Impulse);
    }

    void PickNewRandomDest() {
        randomDest = currentPos + Random.insideUnitCircle.normalized * roamRadius;
    }
}
