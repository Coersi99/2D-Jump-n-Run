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

        /*
        if (Input.GetKey(KeyCode.A) && transform.position.x > maxLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = facingLeft;
            isFacingRight = false;
            animator.SetFloat("Speed", 1);
        }
        else if (Input.GetKey(KeyCode.D) && transform.position.x < maxRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = facingRight;
            isFacingRight = true;
            animator.SetFloat("Speed", 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isDuck", true);
        }else{
            animator.SetBool("isDuck", false);
        }*/


    }

    public void OnCrouching (bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    /*
    private bool isGrounded()
    {
        float extraHeight = 0.05f;
        float sideBuffer = 0.01f;
        float size = cc.bounds.size.x;
        Vector3 startPos = cc.bounds.center + Vector3.right * size / 2 + Vector3.left * sideBuffer;
        RaycastHit2D rh1 = Physics2D.Raycast(startPos, Vector2.down, cc.bounds.extents.y + extraHeight, platformLayerMask);
        startPos = cc.bounds.center + Vector3.left * size / 2 + Vector3.right * sideBuffer;
        RaycastHit2D rh2 = Physics2D.Raycast(startPos, Vector2.down, cc.bounds.extents.y + extraHeight, platformLayerMask);
        Debug.DrawRay(cc.bounds.center + Vector3.right * size / 2 + Vector3.left * sideBuffer, Vector2.down * (cc.bounds.extents.y + extraHeight));
        return rh1.collider != null || rh2.collider != null;
    }*/
}
