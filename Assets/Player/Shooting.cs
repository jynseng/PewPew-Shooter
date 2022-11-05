using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Camera cam;

    Vector2 mousePos;
    private Vector2 toMouse;
    private Vector2 centerPoint;

    // Gun specs
    [SerializeField] float bulletForce = 30f;
    [SerializeField] float bulletSpread = 0.15f;
    [SerializeField] float fireRate = 0.2f; // Seconds between shots

    [SerializeField] float gunRadius = 1f;
    [SerializeField] AudioSource shootSound = null;
    
    private float shootCounter;

    // Update is called once per frame
    void Update() {
        if (!PauseMenu.isPaused) {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            centerPoint = transform.position; // Center of player
            toMouse = (mousePos - centerPoint).normalized; // Vector2 between cursor and player center
            var offset = toMouse * gunRadius;
            firePoint.position = centerPoint + offset; // Move firepoint to closest point to cursor
            
            float rot_z = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg; // Rotate arrow so always pointing out
            firePoint.rotation = Quaternion.Euler(0f, 0f, rot_z);
            
            if(Input.GetButton("Fire1")) { Shoot(); }
            if (shootCounter > 0) { shootCounter -= Time.deltaTime; }
        }
    }

    void Shoot() {
        if (shootCounter > 0) { return;}
        shootSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 firePointPos2D = new Vector2(firePoint.position.x, firePoint.position.y); // Make firePoint.position a 2D vector
        shootCounter = fireRate;

        // Bullet spread
        float x = Random.Range(-1f, 1f) * bulletSpread;
        float y = Random.Range(-1f, 1f) * bulletSpread;
        Vector2 direction = (firePointPos2D-centerPoint);
        direction = new Vector2(direction.x + x, direction.y + y);

        rb.AddForce((direction).normalized * bulletForce, ForceMode2D.Impulse);
    }

    public Vector2 FaceDirection () {return toMouse;}

}
