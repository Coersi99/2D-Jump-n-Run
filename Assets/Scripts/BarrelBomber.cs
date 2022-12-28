using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBomber : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public float attackRange = 1f;

    Rigidbody2D rb;
    BoxCollider2D bc;
    //public Animator animator;

    GameObject playerRef;

    [SerializeField] bool isFacingRight = true;
    [SerializeField] private float speed = 1f;
    [SerializeField] int touchDamage = 1;

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

    }

    //Check for collision with player and then call TakeDamage()
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player"){
            Vector2 directionToTarget = (other.gameObject.transform.position - transform.position).normalized;  //Determine direction for knockback
            other.gameObject.GetComponent<HeartSystem>().TakeDamage(touchDamage, directionToTarget.x);
        }
    }
}
