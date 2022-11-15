using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    Rigidbody2D rb;
    BoxCollider2D bc;
    [SerializeField] bool facingRight = true;
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
        moveBackAndForth();
    }

    private void moveBackAndForth()
    {
        if (reachedEdge(facingRight) || bumpedIntoWall(facingRight))
        {
            facingRight = !facingRight;
        }
        int direction = -1;
        if (facingRight)
        {
            direction = 1;
        }
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    private bool bumpedIntoWall(bool facingRight)
    {
        float extraDepth = 0.05f;
        float sideBuffer = 0.01f;
        float size = bc.bounds.size.y;
        Vector3 dir;
        if (facingRight)
        {
            dir = Vector3.right;
        }
        else
        {
            dir = Vector3.left;
        }
        Vector3 startPos = bc.bounds.center + Vector3.up * size / 2 + Vector3.down * sideBuffer;
        RaycastHit2D rhUp = Physics2D.Raycast(startPos, dir, bc.bounds.extents.x + extraDepth, platformLayerMask);
        // Debug.DrawRay(startPos, dir * (bc.bounds.extents.y + extraDepth));
        startPos = bc.bounds.center + Vector3.down * size / 2 + Vector3.up * sideBuffer;
        RaycastHit2D rhDown = Physics2D.Raycast(startPos, dir, bc.bounds.extents.x + extraDepth, platformLayerMask);
        // Debug.DrawRay(startPos, dir * (bc.bounds.extents.y + extraDepth));
        return rhUp.collider != null || rhDown.collider != null;
    }

    private bool reachedEdge(bool facingRight)
    {
        float extraHeight = 0.05f;
        float sideBuffer = 0.01f;
        float size = bc.bounds.size.x;
        Vector3 dirBig;
        Vector3 dirSmall;
        if (facingRight)
        {
            dirBig = Vector3.right;
            dirSmall= Vector3.left;
        }
        else
        {
            dirBig = Vector3.left;
            dirSmall = Vector3.right;
        }
        Vector3 startPos = bc.bounds.center + dirBig * size / 2 + dirSmall * sideBuffer;
        RaycastHit2D rh = Physics2D.Raycast(startPos, Vector2.down, bc.bounds.extents.y + extraHeight, platformLayerMask);
        // Debug.DrawRay(startPos, Vector2.down * (bc.bounds.extents.y + extraHeight));
        return rh.collider == null;
    }
}
