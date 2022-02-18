using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //EnemyHealth health = null;
    
    [SerializeField] float scanRadius = 5f;
    [SerializeField] float chaseRadius = 50f;
    [SerializeField] float moveSpeed = 1f;
    
    bool targeting = false;
    Vector3 roamDirection;
    float distanceToTarget = Mathf.Infinity;
    Transform target = null;

    void Start() {
        //health = GetComponent<EnemyHealth>();
        target = FindObjectOfType<PlayerHealth>().transform;
        targeting = false;
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        roamDirection = new Vector3(randomDir.x, randomDir.y, 0);
    }

    void Update() {
        distanceToTarget = Vector2.Distance(target.position, transform.position);

        if (targeting && distanceToTarget > chaseRadius) {targeting = false;}  // If player out of reach while targeting, stop chasing

        if (targeting || distanceToTarget <= scanRadius) { // If player target comes within scan radius, start chasing
            targeting = true; 
            ChasePlayer();
        } else {
            transform.position += roamDirection * Time.deltaTime * moveSpeed;
        }
    }

    void ChasePlayer() {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        InvokeRepeating("Shoot", 0.5f, 1f);
    }

    void Shoot() {
        // Shoot player
    }
}
