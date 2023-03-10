using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //(Charged) shot stuff
    public bool canShoot = true;
    [SerializeField] public float shootCooldown;
    [SerializeField] private float chargeSpeed;
	[SerializeField] private float chargeLimit;
    private float chargeTime;
	private bool isCharging;
    private bool canChargeFlash = true;

    //Audio stuff
    [SerializeField] private AudioSource dashSoundEffect;
    [SerializeField] private AudioSource chargeEffect;
    [SerializeField] private AudioSource fullyChargedEffect;

    //Dash params
    private bool canDash = true;
	private bool isDashing;
	private float dashingPower = 25f;
	private float dashingTime = 0.2f;
	private float dashingCooldown = 0.7f;
    [SerializeField] private TrailRenderer tr;
    private Rigidbody2D rb;

    //Knockback stuff
    public float knockBackTotalTime;
    public float knockbackForce = 6f;
    public float knockbackCounter;
    public bool knockFromRight;

    //gamemaster stuff
    private GameMaster gm;

    //Bool used for resuming game and thus executing certain actions where button was held down
    private bool gameResumed = false;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        rb  = GetComponent<Rigidbody2D>();

        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        gm.firstCheckPointPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenu.GameIsPaused){         //disable all actions when game is paused
            gameResumed = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.H) && !HeartSystem.Instance.playerDead) //Load X Menu
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("X");
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

        if((Input.GetKeyUp(KeyCode.K)  && !isDissolving && knockbackCounter <= 0 && canShoot) || (gameResumed && !isDissolving && chargeTime >= chargeLimit))     //release charged shot when K released or game resumed
        {
            gameResumed = false;
            animator.SetBool("isFullyCharged", false);
            chargeEffect.Stop();
            isCharging = false;
            canChargeFlash = true;
            StartCoroutine(shoot());
        }

        if(isDashing)       //disable all other actions when dashing
        {
            return;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //This way, several buttons work ("A and D", "Leftarrow and Rightarrow", etc.)

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); //Change speed triggers run animation

        if (Input.GetButtonDown("Jump"))  //Trigger jump for CharacterController2D
        {
            jump = true;
            animator.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Crouch") && !isCharging)    //Enable crouch for CharacterController2D
        {
            crouch = true;
        } else if(Input.GetButtonUp("Crouch") || gameResumed)
        {
            crouch = false;
            gameResumed = false;
        }

        if(Input.GetKey(KeyCode.K) && knockbackCounter <= 0 && chargeTime < chargeLimit && canShoot && !controller.m_wasCrouching && !isDissolving) //Charge shot
        {
            isCharging = true;
            animator.SetBool("isCharge", true);
            if(isCharging)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
            }

            if(chargeTime >= chargeLimit/2 && canChargeFlash)
            {
                GetComponent<SimpleCharge>().chargeFlash();
                chargeEffect.Play();
                canChargeFlash = false;
            }

            if(chargeTime >= chargeLimit)
            {
                animator.SetBool("isFullyCharged", true);
                chargeEffect.Stop();
                fullyChargedEffect.Play();
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && !crouch && knockbackCounter <= 0)
        {
            StartCoroutine(dash());
        }

        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(transform.position.x < 89)
            {
                transform.position = new Vector3(89, -39, 0);
            }else if (transform.position.x < 218)
            {
                transform.position = new Vector3(218, -30, 0);
            }
            else
            {
                transform.position = new Vector3(310, -22, 0);
            }
            
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
        animator.SetBool("IsCrouching", isCrouching); //Trigger crouch animation when crouching 
    }

    public IEnumerator dash()
	{
		if(canDash)
        {
            dashSoundEffect.Play();
            canDash = false;
			isDashing = true;
			float originalGravity = rb.gravityScale;
			rb.gravityScale = 0f;

            if(transform.rotation.y >= 0)
            {
                rb.velocity = new Vector2(dashingPower, 0f);
            } else 
            {
                rb.velocity = new Vector2(-dashingPower, 0f);
            }
			
			tr.emitting = true;
			yield return new WaitForSeconds(dashingTime);
			tr.emitting = false;
			rb.gravityScale = originalGravity;
			isDashing = false;
			yield return new WaitForSeconds(dashingCooldown);
			canDash = true;
        }
	}

    public IEnumerator shoot()
    {
        if(chargeTime < chargeLimit)
        {
            canShoot = false;
            controller.shootUncharged(); 
            animator.SetBool("isCharge", false);
            chargeTime = 0;
            yield return new WaitForSeconds(shootCooldown);
            canShoot = true;
        }else
        {
            canShoot = false;
            controller.releaseCharge();
            isCharging = false;
            animator.SetBool("isCharge", false);
            chargeTime = 0;
            yield return new WaitForSeconds(shootCooldown);
            canShoot = true;
        }
    }
}

