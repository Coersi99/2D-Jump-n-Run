using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public Animator animator;
    public int health = 10000;
    public bool isVulnerable = true;

    public HealthBar healthBar;

    void Start()
    {
        healthBar.SetMaxHealth(health);
    }


    public void TakeDamage(int damage)
    {
        if (!isVulnerable) return;

        health -= damage;
        healthBar.SetHealth(health);

        if (health > 0)
        {
            GetComponent<SimpleFlash>().Flash();
        }
        else if (health <= 0)
        {
            Die();
            animator.SetBool("dead", true);
            //Destroy(gameObject, 1f);
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //GetComponent<Boss>().enabled = false;
            //<FieldOfView>().enabled = false;
            //<EnemyWeapon>().enabled = false;
            //<CircleCollider2D>().enabled = false;
            //<BoxCollider2D>().enabled = false;
        }
    }

    void Die()
    {
        animator.SetBool("dead", true);
        AudioManager.Instance.playGawdEffect();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        this.enabled = false;
        
        
        
        //animator.SetBool("Death", true);
        //Destroy(gameObject);
    }

}