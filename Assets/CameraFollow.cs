using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 velocity = Vector3.zero;
    public float dampTime = 0.15f;
    public Transform circle;

    void FixedUpdate() {
        
        if (player) {
            Vector3 viewportPoint = GetComponent<Camera>().WorldToViewportPoint(player.position);
            Vector3 mousePos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3 target = new Vector3(player.position.x, player.position.y, 0f);
            if (mousePos.magnitude > 0 && Mathf.Abs(mousePos.x) < Mathf.Abs(Screen.width) && Mathf.Abs(mousePos.y) < Mathf.Abs(Screen.width)) {
                target = Vector3.Lerp(player.position, mousePos, .5f); // target between player and mouse position
                target.z = 0f;
            }
            Vector3 camPoint = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            camPoint.z = 0f;
            Vector3 delta = target - camPoint;
            Vector3 destination = transform.position + delta;
            this.transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}
