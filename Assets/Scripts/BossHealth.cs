using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public Animator animator;
    public int health = 10000;
    public bool isVulnerable = true;

    public float aggroRange = 10f;

    public HealthBar healthBar;
    Transform player;
    Rigidbody2D rb;
    bool startedBossMusic = false;
    
    void Start()
    {
        healthBar.SetMaxHealth(health);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Vector2.Distance(player.position, rb.position) <= aggroRange)
        {
            healthBar.gameObject.SetActive(true);
            if (!startedBossMusic)
            {
                AudioManager.Instance.playBossMusic();
                startedBossMusic = true;
            }
        }
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
        }
    }

    void Die()
    {
        animator.SetBool("dead", true);
        AudioManager.Instance.playBossDeathEffect();
        
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        this.enabled = false;

        Invoke("Destroy", 1.1f);

    }

    void Destroy()
    {
        Destroy(gameObject);
        healthBar.gameObject.SetActive(false);
        AudioManager.Instance.playVictoryMusic();
    }
}