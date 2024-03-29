using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("How much the camera moves towards the crosshair")]
    [SerializeField] private float camMoveFactor = 2.5f;
    [Tooltip("How fast camera moves towards crosshair")]
    [SerializeField] private float dampTime = 0.15f;
    [SerializeField] Transform player;

    private Vector3 velocity = Vector3.zero;
    private bool mouseInFrame = false;

    void FixedUpdate() {
        if (player && player.GetComponent<PlayerHealth>().isAlive) {
            // Move camera 1:1 with player movement
            Vector3 target = new Vector3(player.position.x, player.position.y, 0f);
            Vector3 camPoint = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            camPoint.z = 0f;
            Vector3 delta = target - camPoint;
            Vector3 destination = transform.position + delta;
            this.transform.position = destination;
            
            // Check if mouse cursor is within screen boundaries
            Vector3 mousePosScreen = Input.mousePosition;
            Vector3 mousePos = GetComponent<Camera>().ScreenToWorldPoint(mousePosScreen);
            if (mousePosScreen.x < Screen.width && mousePosScreen.x > 0 && mousePosScreen.y < Screen.height && mousePosScreen.y > 0) {
                mouseInFrame = true;
            } else {
                mouseInFrame = false;
            }

            // Nudge camera with cursor/crosshair (with damping)
            if (mousePos.magnitude > 0 && mouseInFrame) {
                //target = Vector3.Lerp(player.position, mousePos, .9f); // Point between player and mouse position
                //target = mousePos;
                mousePos.z = 0f;
                delta = mousePos - camPoint;
                destination = transform.position + delta*camMoveFactor;
                this.transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }
    }
}
