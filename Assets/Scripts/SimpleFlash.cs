using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlash : MonoBehaviour
{
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;

    //The SpriteRenderer that should flash.
    private SpriteRenderer spriteRenderer;

    //The material that was in use, whe the script startet
    private Material originalMaterial;

    //The currently running coroutine.
    private Coroutine flashRoutine;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private IEnumerator FlashRoutine()
    {
        for(int i = 0;i<4;i++){
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(duration/10);
            spriteRenderer.material = originalMaterial;
            yield return new WaitForSeconds(duration/10);
        }

        flashRoutine = null;
    }

    public void Flash()
    {
        if(flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }
}
