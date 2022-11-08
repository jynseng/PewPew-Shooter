using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private Material whiteMat;
    [SerializeField] private Material originalMat;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float duration = 0.07f;
    private Coroutine flashRoutine;

    // Start is called before the first frame update
    void Start() {
        originalMat = spriteRenderer.material;
    }

    public void Flash() {
        if (flashRoutine != null) {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine() {
        spriteRenderer.material = whiteMat;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMat;
        flashRoutine = null;
    }
}
