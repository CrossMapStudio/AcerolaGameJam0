using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Hold", menuName = "States/Player/Hold")]
public class PlayerState_Hold : BaseState
{
    public override void onEnter()
    {
        PlayerController.Get_Controller.Get_PlayerAnimator.Play("Player_Idle");
    }

    public override void onExit()
    {

    }

    public override void onFixedUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }

    public override bool checkValid()
    {
        return true;
    }
}
