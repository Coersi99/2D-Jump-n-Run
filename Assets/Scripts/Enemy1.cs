using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    //Shader 
    Material material;
    float fade = 0f;
    bool isDissolving = true;
    Color dissolve_color = new Color(0.56f, 0.2f, 0, 1);

    public float attackRange = 1f;

    Transform player;
    Rigidbody2D rb;
    BoxCollider2D bc;
    CircleCollider2D cc;
    EnemyWeapon weapon;

    [SerializeField] bool isFacingRight = true;
    [SerializeField] private float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        cc = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weapon = GetComponent<EnemyWeapon>();
        material = GetComponent<SpriteRenderer>().material;
        material.SetColor("_Color", dissolve_color);
        material.SetFloat("_Scale", 40f);
    }

    // Update is called once per frame
    void Update()
    {

        // dissolve shader
        if (isDissolving)
        {
            fade += Time.deltaTime/2;
            if(fade >= 1f)
            {
                fade = 1;
                isDissolving = false;
            }
            material.SetFloat("_Fade", fade);
        }

        moveBackAndForth();

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            weapon.Attack();
        }

    }

    private void moveBackAndForth()
    {
        if (reachedEdge(isFacingRight) || bumpedIntoWall(isFacingRight))
        {
            //Visually change direction of Sprite
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

            isFacingRight = !isFacingRight;
        }
        int direction = -1;
        if (isFacingRight)
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
        Vector3 dir = getDirection();
        Vector3 startPos = bc.bounds.center + Vector3.up * size / 2 + Vector3.down * sideBuffer;
        RaycastHit2D rhUp = Physics2D.Raycast(startPos, dir, bc.bounds.extents.x + extraDepth, platformLayerMask);
        Debug.DrawRay(startPos, dir * (bc.bounds.extents.y + extraDepth));
        startPos = bc.bounds.center + Vector3.down * size / 2 + Vector3.up * sideBuffer;
        RaycastHit2D rhDown = Physics2D.Raycast(startPos, dir, bc.bounds.extents.x + extraDepth, platformLayerMask);
        Debug.DrawRay(startPos, dir * (bc.bounds.extents.y + extraDepth));
        return rhUp.collider != null || rhDown.collider != null;
    }

    private bool reachedEdge(bool facingRight)
    {
        float extraHeight = 0.05f;
        float sideBuffer = 0.01f;
        float size = cc.bounds.size.x;
        Vector3 dirBig;
        Vector3 dirSmall;
        if (isFacingRight)
        {
            dirBig = Vector3.right;
            dirSmall= Vector3.left;
        }
        else
        {
            dirBig = Vector3.left;
            dirSmall = Vector3.right;
        }
        Vector3 startPos = cc.bounds.center + dirBig * size / 2 + dirSmall * sideBuffer;
        RaycastHit2D rh = Physics2D.Raycast(startPos, Vector2.down, cc.bounds.extents.y + extraHeight, platformLayerMask);
        // Debug.DrawRay(startPos, Vector2.down * (cc.bounds.extents.y + extraHeight));
        return rh.collider == null;
    }

    public Vector3 getDirection()
    {
        if (isFacingRight)
        {
            return Vector3.right;
        }
        else
        {
            return Vector3.left;
        }
    }
}
