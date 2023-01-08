using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] bool smart = false;
    public Animator animator;
    public int health = 500;
    public bool isVulnerable = true;

    //Enemy drops
    [Range(0.0f, 1f)] public float heartDropChance;
    public GameObject heart; 

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
        Debug.Log(selector);
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
        if (Random.value < heartDropChance) Instantiate(heart ,new Vector3(transform.position.x, transform.position.y-0.3f, 1), heart.transform.rotation);
        Destroy(gameObject);
    }
}
