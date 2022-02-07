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

    public float bulletForce = 10f;

    [SerializeField] AudioSource shootSound = null;
    [SerializeField] float gunRadius = 1;

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        centerPoint = transform.position; // Center of player
        toMouse = (mousePos - centerPoint).normalized; // Vector2 between cursor and player center
        var offset = toMouse * gunRadius;
        firePoint.position = centerPoint + offset; // Move firepoint to closest point to cursor
        
        float rot_z = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0f, 0f, rot_z);
        
        if(Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    void Shoot() {
        shootSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 aimDir = (mousePos - rb.position);
        rb.AddForce(aimDir.normalized * bulletForce, ForceMode2D.Impulse);
    }
}
