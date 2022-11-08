using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldReflect : MonoBehaviour
{
    // Reflect enemy fire back at source without taking damage

    [SerializeField] AudioSource shieldSFX;
    [SerializeField] SpriteRenderer shieldSprite;
    [SerializeField] private float shieldDuration;

    private bool isShielding;

    void Start() {
        shieldSprite.enabled = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isShielding) {
            StartCoroutine(ShieldCoroutine());
        }
    }

    private IEnumerator ShieldCoroutine() {
        shieldSprite.enabled = true;
        shieldSFX.Play();
        // Absorb bullet?
        // Enable circle collider2d
        yield return new WaitForSeconds(shieldDuration);
        shieldSprite.enabled = false;
        // Disable circle collider2d;
    }
}
