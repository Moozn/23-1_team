using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateMachineBehaviour
{
    private Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();
        if (stateInfo.IsTag("Idle")) player.StateChange(State.Idle);
        else if (stateInfo.IsTag("Attack")) player.StateChange(State.Attack);
        else if (stateInfo.IsTag("Dash")) player.StateChange(State.Dash); //상태 변경
    }
}
