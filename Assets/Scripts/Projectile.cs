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

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }



    public void setAttributes(int id, bool facingRight)
    {
        setId(id);
        if (facingRight)
            direction = Vector3.right;
        else
            direction = Vector3.left;
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
        projectilePool.GetComponent<ProjectilePool>().DestroyObject(id);
    }
}
