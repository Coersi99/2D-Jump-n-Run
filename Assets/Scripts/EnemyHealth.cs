using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] bool smart = false;
    public Animator animator;
    public int health = 500;
    public bool isVulnerable = true;

    public void TakeDamage(int damage)
    {
        if (smart) GetComponent<FieldOfView>().CanSeePlayer = true;
        if(!isVulnerable) return;

        health -= damage;

        if(health > 0)
        {
            GetComponent<SimpleFlash>().Flash();
        }
        else
        {
            playRandomDeathSound();
            animator.SetBool("isDead", true);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<BarrelKnight>().enabled = false;
            GetComponent<FieldOfView>().enabled = false;
            GetComponent<EnemyWeapon>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        } 
    }

    private void playRandomDeathSound()
    {
        int selector = Random.Range(0,3);
        if(selector == 0)
        {
            AudioManager.Instance.playGawdEffect();
        }else if(selector == 1)
        {
            AudioManager.Instance.playEnemyDeath1Effect();
        }else
        {
            AudioManager.Instance.playEnemyDeath2Effect();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
