using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int attackDamage = 1;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;
    public float knockback = 50f;

    public void Attack()
    {
        //Get pos of enemy
        Vector3 pos = transform.position;
        
        //add x offset, flip if facing left
        if(transform.localScale.x < 0)
        {
            pos += transform.right * attackOffset.x;
        }else{
            pos += transform.right * -attackOffset.x;
        }
        
        //check collision with player
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if(colInfo != null && colInfo.gameObject.tag == "Player") 
        {
            Vector2 directionToTarget = (colInfo.gameObject.transform.position - transform.position).normalized;
            HeartSystem.Instance.TakeDamage(attackDamage, directionToTarget.x);
        }

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;

        Vector3 pos = transform.position;
        pos += transform.right * -attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

}
