using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharge : MonoBehaviour
{
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material chargeFlashMaterial;

    //The SpriteRenderer that should flash.
    private SpriteRenderer spriteRenderer;

    //The material that was in use, whe the script startet
    private Material originalMaterial;

    //The currently running coroutine.
    private Coroutine flashRoutine;

    //Cooldown is the player's shoot cooldown
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        cooldown = GetComponent<Player>().shootCooldown;
    }

    private IEnumerator FlashRoutine()
    {
        for(int i = 4; i<=32; i = 2*i)      //loading shot
        {
            spriteRenderer.material = chargeFlashMaterial;
            yield return new WaitForSeconds(cooldown/i);
            spriteRenderer.material = originalMaterial;
            yield return new WaitForSeconds(cooldown/i);
        }  

        spriteRenderer.material = chargeFlashMaterial;  //Now the shot is loaded
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }

    public void chargeFlash()
    {
        if(flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }
}
