using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 velocity = Vector3.zero;
    public float dampTime = 0.15f;
    [SerializeField] private float camMoveFactor = 2.5f;

    void FixedUpdate() {
        
        if (player) {
            // Move camera 1:1 with player movement
            Vector3 target = new Vector3(player.position.x, player.position.y, 0f);
            Vector3 camPoint = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            camPoint.z = 0f;
            Vector3 delta = target - camPoint;
            Vector3 destination = transform.position + delta;
            this.transform.position = destination;
            
            // Nudge camera with cursor/crosshair (with damping)
            Vector3 mousePos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.magnitude > 0 && Mathf.Abs(mousePos.x) < Mathf.Abs(Screen.width) && Mathf.Abs(mousePos.y) < Mathf.Abs(Screen.width)) {
                //target = Vector3.Lerp(player.position, mousePos, .9f); // Point between player and mouse position
                target = mousePos;
                target.z = 0f;
                delta = target - camPoint;
                destination = transform.position + delta*camMoveFactor;
                this.transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }
    }
}
