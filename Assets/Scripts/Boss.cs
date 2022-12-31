using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;

    public bool isFlipped = false;
    [SerializeField] int touchDamage = 1;

    public HealthBar healthBar;


    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    //Check for collision with player and then call TakeDamage()
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector2 directionToTarget = (other.gameObject.transform.position - transform.position).normalized;  //Determine direction for knockback
            other.gameObject.GetComponent<HeartSystem>().TakeDamage(touchDamage, directionToTarget.x);
        }
    }

}
