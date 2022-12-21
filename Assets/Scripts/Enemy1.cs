using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FieldOfView;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public float attackRange = 1f;

    Rigidbody2D rb;
    BoxCollider2D bc;
    CircleCollider2D cc;
    FieldOfView fieldOfView;
    EnemyWeapon weapon;
    //public Animator animator;

    public GameObject playerRef;

    [SerializeField] bool isFacingRight = true;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] int touchDamage = 1;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        cc = GetComponent<CircleCollider2D>();
        weapon = GetComponent<EnemyWeapon>();
        fieldOfView = GetComponent<FieldOfView>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (fieldOfView.GetCurrentState() == enemyState.idleState)
        {
            moveBackAndForth();
        }
        else if (fieldOfView.GetCurrentState() == enemyState.aggressiveState)
        {
            GoToPlayer();
            if(bumpedIntoWall(isFacingRight)){
                animator.SetBool("isAttack", false);
            }
        }

    }

    private void GoToPlayer()
    {
        float playerEnemyPosXDiff = playerRef.transform.position.x - this.transform.position.x;
        if (Mathf.Abs(playerEnemyPosXDiff) >= 0.5f)
        {
            if (playerEnemyPosXDiff < 0)
            {
                //Visually change direction of Sprite
                if (isFacingRight)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
                isFacingRight = false;
            }
            else if (playerEnemyPosXDiff > 0)
            {
                //Visually change direction of Sprite
                if (!isFacingRight)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
                isFacingRight = true;
            }
            int direction = -1;
            if (isFacingRight)
            {
                direction = 1;
            }
            rb.velocity = new Vector2(direction * attackSpeed, rb.velocity.y);
            //animator.SetBool("isAttack", true);
        }
    }

    //Check for collision with player and then call TakeDamage()
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<HeartSystem>().TakeDamage(touchDamage, transform.localScale.x);
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
        float size = cc.bounds.size.y;
        Vector3 dir = getDirection();
        Vector3 startPos = cc.bounds.center + Vector3.up * size / 2 + Vector3.down * sideBuffer;
        RaycastHit2D rhUp = Physics2D.Raycast(startPos, dir, cc.bounds.extents.x + extraDepth, platformLayerMask);
        //Debug.DrawRay(startPos, dir * (cc.bounds.extents.y + extraDepth));
        startPos = cc.bounds.center + Vector3.down * size / 3 + Vector3.up * sideBuffer;
        RaycastHit2D rhDown = Physics2D.Raycast(startPos, dir, cc.bounds.extents.x + extraDepth, platformLayerMask);
        Debug.DrawRay(startPos, dir * (cc.bounds.extents.y + extraDepth));
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
        //Debug.DrawRay(startPos, Vector2.down * (cc.bounds.extents.y + extraHeight));
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
