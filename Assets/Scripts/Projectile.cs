using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    BoxCollider2D bc;
    public Rigidbody2D rb;

    [SerializeField] int id;
    [SerializeField] float speed = 2f;
    [SerializeField] int damage = 100;
    private float direction;
    private ProjectilePool projectilePool;
    [SerializeField] float timeToLiveLimit = 10f;
    private float timeToLive;
    private bool hasNotCollided;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(direction * Time.deltaTime * speed);
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0)
        {
            projectilePool.GetComponent<ProjectilePool>().DestroyObject(id);
        }
    }



    public void setAttributes(int id, bool facingRight)
    {
        hasNotCollided = true;
        setId(id);
        if (facingRight)
        {
            direction = 1;
            if(transform.localScale.x<0) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else
        {
            direction = -1;
            if(transform.localScale.x>=0) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        timeToLive = timeToLiveLimit;
        rb.velocity = new Vector2(direction * speed, 0);
    }

    public void setId(int id)
    {
        this.id = id;
    }

    public void setProjectilePool(ProjectilePool pool)
    {
        projectilePool = pool;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasNotCollided)
        {
            if (collision.gameObject.CompareTag("BarrelKnight"))
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                AudioManager.Instance.playEnemyHitEffect();
            }
            else if (collision.gameObject.CompareTag("StupidKnight"))
            {
                collision.gameObject.GetComponent<StupidKnightHealth>().TakeDamage(damage);
                AudioManager.Instance.playEnemyHitEffect();
            }
            else if (collision.gameObject.CompareTag("BarrelBomber"))
            {
                collision.gameObject.GetComponent<BarrelBomberHealth>().TakeDamage(damage);
                AudioManager.Instance.playEnemyHitEffect();
            }
            else if (collision.gameObject.CompareTag("Boss"))
            {
                collision.gameObject.GetComponent<BossHealth>().TakeDamage(damage);
                AudioManager.Instance.playEnemyHitEffect();
            }
            if (!collision.gameObject.CompareTag("Player"))
            {
                projectilePool.GetComponent<ProjectilePool>().DestroyObject(id);
                hasNotCollided = false;
            }
        }
        
    }
}
