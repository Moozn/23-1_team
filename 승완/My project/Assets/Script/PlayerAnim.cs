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
    public void Attack()
    {
        if (anim) anim.SetTrigger("Attack");
    }
    public void Move(float move)
    {
       if(anim) anim.SetFloat("Move", move);
    }
}
