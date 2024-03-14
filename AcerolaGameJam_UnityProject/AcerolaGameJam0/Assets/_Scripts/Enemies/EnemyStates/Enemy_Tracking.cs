using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Needs Path Finding ---
[CreateAssetMenu(fileName = "Basic Enemy Tracking State", menuName = "Enemy State/Tracking Enemy State")]
public class Enemy_Tracking : Enemy_BaseState
{
    [SerializeField] private float Speed = 1.5f;
    //General Movement has One Animation ---
    [SerializeField] private string AnimationClip_Name;

    private float RangeCheck;

    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        RangeCheck = Driver.Get_ActiveOffesiveStateGroup.Range;
        Driver.ChangeToHitState_Channel.AddListener(Enter_HitState);
        Driver.Get_Animator.Play(AnimationClip_Name, 0, 0);
    }

    public override void onExit()
    {
        Driver.ChangeToHitState_Channel.RemoveListener(Enter_HitState);
    }

    public override void onFixedUpdate()
    {
        Vector3 direction = (Driver.Get_Target.position - Driver.Get_Position).normalized;
        Driver.Get_EnemyRB.MovePosition(Driver.Get_EnemyRB.transform.position + (direction * Speed * Time.deltaTime));
        if (direction.x < 0)
        {
            Driver.Get_Renderer.flipX = true;
        }
        else
        {
            Driver.Get_Renderer.flipX = false;
        }
    }

    public override void onInactiveUpdate()
    {
  
    }

    public override void onLateUpdate()
    {
    
    }

    public override void onUpdate()
    {
        //Based on a Recovery Timer ---
        if (Vector2.Distance(Driver.Get_Target.position, Driver.Get_EnemyRB.position) <= RangeCheck)
        {
            Driver.Offensive_StateChange();
        }
    }

    public void Enter_HitState()
    {
        Driver.EnemyStateMachine.changeState(Driver.Enemy_States[2]);
    }
}
