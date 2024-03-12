using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Enemy Attack State", menuName = "Enemy State/Attack Set Target Enemy State")]
public class Enemy_Attack : Enemy_BaseState
{
    //General Movement has One Animation ---
    [SerializeField] private string Start_Attack_AnimationClipName;
    [SerializeField] private string Launch_Attack_AnimationClipName;
    [SerializeField] private string Finish_Attack_AnimationClipName;

    [SerializeField] private float Speed;
    [SerializeField] private float DamageRadius;
    [SerializeField] private float Damage;
    [SerializeField] private LayerMask Damage_Layer;
    private bool LaunchAttack;
    Vector2 TargetDirection;

    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        Driver.On_Init.OnEventRaised.AddListener(Launch_Attack);
        Driver.On_Event.OnEventRaised.AddListener(Finish_Attack);
        Driver.On_Finish.OnEventRaised.AddListener(End_State);
        Driver.ChangeToHitState_Channel.AddListener(Enter_HitState);

        Start_Attack();
    }

    public override void onExit()
    {
        Driver.On_Init.OnEventRaised.RemoveListener(Launch_Attack);
        Driver.On_Event.OnEventRaised.RemoveListener(Finish_Attack);
        Driver.On_Finish.OnEventRaised.RemoveListener(End_State);
        Driver.ChangeToHitState_Channel.RemoveListener(Enter_HitState);
    }

    public override void onFixedUpdate()
    {
        if (LaunchAttack)
        {
            Driver.Get_EnemyRB.MovePosition(Driver.Get_EnemyRB.position + (TargetDirection.normalized * Speed * Time.deltaTime));
            var col = Physics2D.OverlapCircle(Driver.Get_EnemyRB.transform.position, DamageRadius, Damage_Layer);
            if (col != null)
            {
                Driver.Player_CombatChannel.RaiseEvent(TargetDirection.normalized, Damage);
                Finish_Attack();
            }
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
        Vector3 direction = (Driver.Get_Target.position - Driver.Get_Position).normalized;
        if (direction.x < 0)
        {
            Driver.Get_Renderer.flipX = true;
        }
        else
        {
            Driver.Get_Renderer.flipX = false;
        }
    }

    public void Start_Attack()
    {
        Driver.Get_Animator.Play(Start_Attack_AnimationClipName, 0, 0);
    }

    public void Launch_Attack()
    {
        //Play Animation
        TargetDirection = (PlayerController.Get_Controller.Get_PlayerRB.transform.position - Driver.transform.position).normalized;
        LaunchAttack = true;
        Driver.Get_Animator.Play(Launch_Attack_AnimationClipName, 0, 0);
    }

    public void Finish_Attack()
    {
        LaunchAttack = false;
        Driver.Get_Animator.Play(Finish_Attack_AnimationClipName, 0, 0);
    }

    public void End_State()
    {
        Driver.EnemyStateMachine.changeState(Driver.Enemy_States[1]);
    }

    public void Enter_HitState()
    {
        Driver.EnemyStateMachine.changeState(Driver.Enemy_States[2]);
    }
}
