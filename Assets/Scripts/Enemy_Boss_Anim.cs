using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_Anim : StateMachineBehaviour
{
    float speed = 4f;
    float attackRange = 1.5f;
    float timeLimitAttack = 1.8f;
    float currTimeAttack;
    
    Transform player;
    Rigidbody2D rb;
    Boss boss;

    int randSkill;
    int limitSkill;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        limitSkill = 2;
        currTimeAttack = 3f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currTimeAttack += Time.deltaTime;
        randSkill = Random.Range(0, limitSkill);

        boss.LookAtPlayer();
        //Move
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);

        
        rb.MovePosition(newPos);
        
        if (!boss.isAngry)
        {
            if (Vector2.Distance(player.position, rb.position) <= attackRange && currTimeAttack >= timeLimitAttack)
            {
                animator.SetTrigger("attack1");

                currTimeAttack = 0;
            }
        }
        else if(boss.isAngry)
        {
            //set value - angry state
            limitSkill = 3;
            timeLimitAttack = 1f;
            speed = 6f;

            if ((Vector2.Distance(player.position, rb.position) <= attackRange) && (currTimeAttack >= timeLimitAttack) )
            {
                currTimeAttack = 0;

                if (randSkill == 0 || randSkill == 1)
                    animator.SetTrigger("attack3");

                if (randSkill == 2)
                    animator.SetTrigger("attack1Angry");
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attack1");
        animator.ResetTrigger("attack2");
        animator.ResetTrigger("attack3");
        animator.ResetTrigger("attack1Angry");
    }


}
