using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    void Awake() {
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update() {
        if (!PauseMenu.isPaused) {
            Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseCursorPos;
        }
    }
}
