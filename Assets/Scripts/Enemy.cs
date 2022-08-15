using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public fields
    public int health = 100;
    public Vector3 attackOffset;
    public int attackDamage = 10;
    public float attackRange = 1f;

    public LayerMask attackMask;
    public Transform player;
    public GameObject deadEffect;
    public GameObject enemyAttackEffect;
    public bool isFlipped = false;
    

    protected virtual void Start()
    {
        player = GameManager.instance.player.transform;
    }
    public virtual void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            Instantiate(enemyAttackEffect, colInfo.transform.position, Quaternion.identity);
            GameManager.instance.player.ReceiveDamage(attackDamage);
        }
    }

    //All Enemy can receiverDamage / die
    public virtual void ReceiveDamage(int dmg)
    {

        health -= dmg;

        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    public virtual void Death()
    {
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void LookAtPlayer()
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

    public void Flip()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
        else if (transform.position.x < player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
    }


    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

}
