using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Player : MonoBehaviour
{
    //PlayerMask should be assigned here
    [SerializeField] private LayerMask platformLayerMask;

    //Actual script for processing movement input
    public CharacterController2D controller;

    //Animator for player animations
    public Animator animator;

    //Parameters for movement control
    [SerializeField] private float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    //Movement boundaries
    [SerializeField] private int maxLeft = -7;
    [SerializeField] private int maxRight = 10;

    //Shader stuff
    Material material;
    float fade = 0f;
    bool isDissolving = true;

    //Audio stuff
    [SerializeField] private AudioSource dashSoundEffect;

    //Dash params
    private bool canDash = true;
	private bool isDashing;
	private float dashingPower = 5f;
	private float dashingTime = 0.2f;
	private float dashingCooldown = 0.7f;
    [SerializeField] private TrailRenderer tr;
    private Rigidbody2D rb;

    //Knockback stuff
    public float knockBackTotalTime;
    public float knockbackForce = 6f;
    public float knockbackCounter;
    public bool knockFromRight;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        rb  = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing){
            return;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //This way, several buttons work ("A and D", "Leftarrow and Rightarrow", etc.)

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); //Change speed triggers run animation

        if (Input.GetButtonDown("Jump"))  //Trigger jump for CharacterController2D
        {
            jump = true;
            animator.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Crouch"))    //Enable crouch for CharacterController2D
        {
            crouch = true;
        } else if(Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        if(Input.GetKeyDown(KeyCode.K) && knockbackCounter <= 0) //Call shoot function in CharacterController2D
        {
            controller.shoot();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && !crouch && knockbackCounter <= 0)
        {
            StartCoroutine(Dash());
        }

        if(isDissolving)    //Activate dissolve shader (inverted) when spawned
        {
            fade += Time.deltaTime/2;
            if(fade >= 1f)
            {
                fade = 1;
                isDissolving = false;
            }
            material.SetFloat("_Fade", fade);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            transform.position = new Vector3(144, -39, 0);

        }

    }

    private void FixedUpdate()
    {
        if(isDashing){
            return;
        }

        if(knockbackCounter <= 0){      //If no knockback applied the player can move freely
            knockbackForce = 10f;
            if (transform.position.x < maxRight && transform.position.x > maxLeft)
            {
                controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);  //Call Move function in CharacterController2D
                jump = false;
            }
        }else{
            if(knockFromRight)
            {
                rb.velocity = new Vector2(knockbackForce, knockbackForce);
                knockbackForce -= 0.7f;
            }else{
                rb.velocity = new Vector2(-knockbackForce, knockbackForce);
                knockbackForce -= 0.7f;
            }

            knockbackCounter -= Time.deltaTime;
        }
        
    }

    public void OnCrouching (bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching); //Trigger crouch animation when crouching (why do I even explain...)
    }

    public IEnumerator Dash()
	{
		if(canDash)
        {
            dashSoundEffect.Play();
            canDash = false;
			isDashing = true;
			float originalGravity = rb.gravityScale;
			rb.gravityScale = 0f;
			rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
			tr.emitting = true;
			yield return new WaitForSeconds(dashingTime);
			tr.emitting = false;
			rb.gravityScale = originalGravity;
			isDashing = false;
			yield return new WaitForSeconds(dashingCooldown);
			canDash = true;
        }
	}
}

