using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Enemy Hit State", menuName = "Enemy State/Hit Enemy State")]
public class Enemy_Hit : Enemy_BaseState
{
    //General Movement has One Animation ---
    [SerializeField] private string AnimationClip_Name;
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        Driver.Get_Animator.Play(AnimationClip_Name, 0, 0);
        Driver.Enemy_Hit_ParticleSystem.Play();

        Driver.ChangeToHitState_Channel.AddListener(Enter_HitState);
        Driver.On_Finish.OnEventRaised.AddListener(Hit_Finished);
    }

    public override void onExit()
    {
        Driver.ChangeToHitState_Channel.RemoveListener(Enter_HitState);
        Driver.On_Finish.OnEventRaised.RemoveListener(Hit_Finished);
    }

    public override void onFixedUpdate()
    {
        Driver.Get_EnemyRB.MovePosition((Vector2)Driver.Get_EnemyRB.transform.position + (Driver.TargetPosition * Time.deltaTime));
    }

    public override void onInactiveUpdate()
    {
       
    }

    public override void onLateUpdate()
    {
       
    }

    public override void onUpdate()
    {
        Driver.TargetPosition = Vector3.Lerp(Driver.TargetPosition, Vector2.zero, Time.deltaTime * 20f);
    }

    //Change State can call driver for decision making ---
    public void Hit_Finished()
    {
        Driver.EnemyStateMachine.changeState(Driver.Enemy_States[1]);
    }

    public void Enter_HitState()
    {
        Driver.EnemyStateMachine.changeState(Driver.Enemy_States[2]);
    }
}
