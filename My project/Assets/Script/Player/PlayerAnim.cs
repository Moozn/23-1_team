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

    public Vector3 forward()
    {
        Debug.Log(anim.transform.forward.normalized);
        if (Mathf.Abs(anim.transform.forward.x) <= 0.1f) return new Vector3(0, 0, anim.transform.forward.normalized.z);
        else if (Mathf.Abs(anim.transform.forward.z) <= 0.1f) return new Vector3(anim.transform.forward.x, 0, 0);
        return anim.transform.forward.normalized;
    }
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
    public void BackRoll()
    {
        if (anim) anim.SetTrigger("BackRoll");
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
