using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public Animator animator;
    public int health = 10000;
    public bool isVulnerable = true;

    public void TakeDamage(int damage)
    {
        if (!isVulnerable) return;

        health -= damage;

        if (health > 0)
        {
            GetComponent<SimpleFlash>().Flash();
        }
        else if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("Death", true);
        Destroy(gameObject);
    }

}