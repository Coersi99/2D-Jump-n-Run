using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShot : MonoBehaviour
{
    BoxCollider2D bc;
    Rigidbody2D rb;

    [SerializeField] float speed = 2f;
    [SerializeField] int damage = 100;
    private float direction;
    [SerializeField] float timeToLiveLimit = 10f;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Invoke("DestroyChargedShot", timeToLiveLimit);
    }

    void DestroyChargedShot()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BarrelKnight"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            AudioManager.Instance.playEnemyHitEffect();
            DestroyChargedShot();
        }
        else if (collision.gameObject.CompareTag("StupidKnight"))
        {
            collision.gameObject.GetComponent<StupidKnightHealth>().TakeDamage(damage);
            AudioManager.Instance.playEnemyHitEffect();
            DestroyChargedShot();
        }
        else if (collision.gameObject.CompareTag("BarrelBomber"))
        {
            collision.gameObject.GetComponent<BarrelBomberHealth>().TakeDamage(damage);
            AudioManager.Instance.playEnemyHitEffect();
            DestroyChargedShot();
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossHealth>().TakeDamage(damage);
            AudioManager.Instance.playEnemyHitEffect();
            DestroyChargedShot();
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            DestroyChargedShot();
        }
    
    }
}
