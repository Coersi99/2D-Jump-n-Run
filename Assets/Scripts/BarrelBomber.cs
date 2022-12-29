using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBomber : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] LayerMask obstructionLayer;

    public float vofRadius = 1f;

    Rigidbody2D rb;
    BoxCollider2D bc;
    public Animator animator;

    GameObject playerRef;

    [SerializeField] bool isFacingRight = true;
    [SerializeField] int touchDamage = 1;
    [Range(1, 360)][SerializeField] float angle = 45f;
    [SerializeField] private AudioSource bombEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        ScanForPlayer();
    }

    private void ScanForPlayer()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, vofRadius, targetLayer);
        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(getDirection(), directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))   // true if ray does not hit obstruction
                {
                    animator.SetBool("isAttack", true);
                }
                else
                {
                    animator.SetBool("isAttack", false);
                }
            }
            else
            {
                animator.SetBool("isAttack", false);
            }
        }
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

    //Check for collision with player and then call TakeDamage()
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player"){
            Vector2 directionToTarget = (other.gameObject.transform.position - transform.position).normalized;  //Determine direction for knockback
            other.gameObject.GetComponent<HeartSystem>().TakeDamage(touchDamage, directionToTarget.x);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, vofRadius);
    }

    void playBombEffect(){
        bombEffect.Play();
    }
}
