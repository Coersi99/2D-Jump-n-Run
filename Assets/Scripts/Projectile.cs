using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    BoxCollider2D bc;

    [SerializeField] int id;
    [SerializeField] float speed = 1f;
    private Vector3 direction;
    private ProjectilePool projectilePool;
    [SerializeField] float timeToLiveLimit = 10f;
    private float timeToLive;
    private bool hasNotCollided;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
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
            direction = Vector3.right;
        else
            direction = Vector3.left;
        timeToLive = timeToLiveLimit;
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
            if (!collision.gameObject.CompareTag("Player"))
            {
                projectilePool.GetComponent<ProjectilePool>().DestroyObject(id);
                hasNotCollided = false;
            }
        }
        
    }
}
