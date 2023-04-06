using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Behaviour : StateMachineBehaviour
{
    private GameObject player;
    private Boss boss;
    private float ftime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = animator.GetComponent<Boss>();
        ftime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.health <= 0)
            return;
        
        ftime += Time.deltaTime;

        if (ftime >= 5)
        {
            int behaviour = Random.Range(1, 4);
            if (behaviour == 1)
            {
                animator.SetTrigger("LeftHand");
            }
            else if (behaviour == 2)
            {
                animator.SetTrigger("RightHand");
            }
            else if (behaviour == 3)
            {
                animator.SetTrigger("BothHand");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
