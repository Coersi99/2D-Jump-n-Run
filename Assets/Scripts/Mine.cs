using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FieldOfView;
using static UnityEngine.GraphicsBuffer;

public class Mine : MonoBehaviour
{
    CircleCollider2D cc;
    public GameObject playerRef;
    public Animator animator;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask obstructionLayer;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float searchRadius = 3.0f;
    [SerializeField] float explodingRadius = 3.0f;
    [SerializeField] ParticleSystem explosion;

    //Determine whether the mine should move or not
    [SerializeField] bool isStationary;

    public int attackDamage = 1;

    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CircleCollider2D>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SurroundCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (active && !isStationary)
        {
            PursuePlayer();
        }
    }

    private IEnumerator SurroundCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f); // only check every 0.2 seconds (more is not necessary)
        while (!active && !isStationary)
        {
            yield return wait;
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, searchRadius, targetLayer);
            if (rangeCheck.Length > 0)
            {
                Transform target = rangeCheck[0].transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))   // true if ray does not hit obstruction
                {
                    active = true;
                    animator.SetBool("isActive", true);
                }
                else
                {
                    active = false;
                }
            }
        }
    }

    private void PursuePlayer()
    {
        Vector2 directionToTarget = (playerRef.transform.position - this.transform.position).normalized;
        transform.Translate(directionToTarget * Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.playExplosionEffect();
        Explode();
        active = false;
    }

    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, explodingRadius, targetLayer);
        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))   // true if ray does not hit obstruction
            {
                HeartSystem.Instance.TakeDamage(attackDamage, directionToTarget.x);
            }
        }
        Destroy(this.gameObject);
    }
}
