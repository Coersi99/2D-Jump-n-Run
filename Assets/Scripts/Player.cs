using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public Animator animator;

    Rigidbody2D rb;
    BoxCollider2D bc;
    CircleCollider2D cc;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int maxLeft = -7;
    [SerializeField] private int maxRight = 10;
    [SerializeField] ProjectilePool projectilePool;

    private bool isFacingRight = true;

    //Movement direction
    [SerializeField] bool spawnFacingLeft;
    private Vector2 facingLeft;
    private Vector2 facingRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        cc = GetComponent<CircleCollider2D>();
        facingRight = new Vector2(transform.localScale.x, transform.localScale.y);
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        if(spawnFacingLeft)
        {
            transform.localScale = facingLeft;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.SetTrigger("Jump");
        }

        // shooting
        if (Input.GetKeyDown(KeyCode.K))
        {
            projectilePool.SpawnObject(isFacingRight, bc.bounds.center, bc.bounds.size.x);
        }
    }

    private void FixedUpdate()
    {
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
    }

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
    }
}
