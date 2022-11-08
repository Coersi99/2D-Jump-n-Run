using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    Rigidbody2D rb;
    BoxCollider2D bc;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private bool isGrounded()
    {
        float extraHeight = 0.05f;
        float sideBuffer = 0.01f;
        float size = bc.bounds.size.x;
        Vector3 startPos = bc.bounds.center + Vector3.right * size / 2 + Vector3.left * sideBuffer;
        RaycastHit2D rh1 = Physics2D.Raycast(startPos, Vector2.down, bc.bounds.extents.y + extraHeight, platformLayerMask);
        startPos = bc.bounds.center + Vector3.left * size / 2 + Vector3.right * sideBuffer;
        RaycastHit2D rh2 = Physics2D.Raycast(startPos, Vector2.down, bc.bounds.extents.y + extraHeight, platformLayerMask);
        // Debug.DrawRay(bc.bounds.center + Vector3.right * size / 2 + Vector3.left * sideBuffer, Vector2.down * (bc.bounds.extents.y + extraHeight));
        return rh1.collider != null || rh2.collider != null;
    }
}
