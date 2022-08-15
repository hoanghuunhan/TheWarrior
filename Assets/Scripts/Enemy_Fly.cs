using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fly : StateMachineBehaviour
{
    public float speed = 2f;
    public float attackRange = 1f;
    public float chaseLength = 12f;
    public float timeLimitAttack = 2f;

    Vector3 origin, flipped;
    Transform player;
    Rigidbody2D rb;
    FlyEnemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<FlyEnemy>();
        origin = enemy.transform.position;

        flipped = enemy.transform.localScale;
        flipped.z *= -1f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Move
        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 originPos = Vector2.MoveTowards(rb.position, origin, speed * Time.fixedDeltaTime);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);


        if (Vector2.Distance(player.position, rb.position) <= chaseLength)
        {
            enemy.LookAtPlayer();
            rb.MovePosition(newPos);
        }

        if (Vector2.Distance(player.position, rb.position) > chaseLength)
        {
            rb.MovePosition(originPos);
            enemy.Flip();
        }

        if (Vector2.Distance(player.position, rb.position) <= attackRange && timeLimitAttack >= 2)
        {
            animator.SetTrigger("attack");
            timeLimitAttack = 0;
        }
        timeLimitAttack += Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attack");
    }


}
