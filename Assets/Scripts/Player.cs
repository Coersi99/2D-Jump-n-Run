using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public CharacterController2D controller;
    public Animator animator;

    float horizontalMove = 0f;
    [SerializeField] private float runSpeed = 40f;
    bool jump = false;
    bool crouch = false;

    //When taking damage, movement should be shortly disabled
    [System.NonSerialized]
    public bool isDamaged = false;

    [SerializeField] private int maxLeft = -7;
    [SerializeField] private int maxRight = 10;

    //Shader 
    Material material;
    float fade = 0f;
    bool isDissolving = true;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        //trigger Jump
        if (Input.GetButtonDown("Jump") && !isDamaged)
        {
            jump = true;
            //rb.velocity = Vector2.up * jumpForce;
            animator.SetTrigger("Jump");
        }

        //enable Crouch
        if (Input.GetButtonDown("Crouch") && !isDamaged)
        {
            crouch = true;

        } else if(Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            controller.shoot();
        }

        // dissolve shader
        if(isDissolving)
        {
            fade += Time.deltaTime/2;
            if(fade >= 1f)
            {
                fade = 1;
                isDissolving = false;
            }
            material.SetFloat("_Fade", fade);
        }
        
    }

    private void FixedUpdate()
    {

        if (transform.position.x < maxRight && transform.position.x > maxLeft && !isDamaged){
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);  
        }
        jump = false;

    }

    public void OnCrouching (bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }
}

