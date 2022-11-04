using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 initCamPos;
    private bool isShaking;
    private float coroutineFreq = 0.018f;

    void Start() {
        mainCamera = Camera.main;
    }

    void Update() {

    }

    public void StartShake() {
        if (!isShaking) {
            StartCoroutine(ShakeCamera());
        }
    }

    private IEnumerator ShakeCamera(float magnitude = 0.12f) {
        isShaking = true;
        float time = 0f, x, y;

        while (time < 0.1f) {
            x = Random.Range(-1f, 1f) * magnitude;
            y = Random.Range(-1f, 1f) * magnitude;

            Vector3 pos = mainCamera.transform.position;
            mainCamera.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
            
            time += coroutineFreq;
            yield return new WaitForSeconds(coroutineFreq);
        }
        isShaking = false;
    }
}
