using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }
 //   private StateMachineExample _stateMachineExample;

    void Start()
    {
        //_stateMachineExample = _animator.GetBehaviour<StateMachineExample>();
    }
    public void Attack()
    {
        if (anim) anim.SetTrigger("Attack");
    }
    public void Move(float move)
    {
       if(anim) anim.SetFloat("Move", move);
    }

    public void Run(bool b)
    {
        if (anim) anim.SetBool("Run", b);
    }

    public void Dash()
    {
        if (anim) anim.SetTrigger("Dash");
    }


    public void Hit()
    {
        if (anim) anim.SetTrigger("Hit");
    }
    public void Die(bool b)
    {
        if (anim) anim.SetBool("Die",b);
    }
}
