using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Animator animator;
    public int health = 500;
    public bool isVulnerable = true;

    public void TakeDamage(int damage)
    {
        if(!isVulnerable) return;

        health -= damage;

        if(health > 0)
        {
            GetComponent<SimpleFlash>().Flash();
        }
        else
        {
            animator.SetBool("isDead", true);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<Enemy1>().enabled = false;
            GetComponent<FieldOfView>().enabled = false;
            GetComponent<EnemyWeapon>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        } 
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
