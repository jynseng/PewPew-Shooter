using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    SpriteRenderer crosshair;

    void Awake() {
        Cursor.visible = false; 
        crosshair = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (!PauseMenu.isPaused) {
            Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseCursorPos;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Enemy") {
            crosshair.color = Color.red;
        } 
    }

    void OnTriggerExit2D() {
        crosshair.color = Color.white;
    }
}
