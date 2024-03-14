using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Range Attack State", menuName = "Enemy State/Range Attack State")]
public class Enemy_RangeAttack : Enemy_BaseState
{
    [SerializeField] private Projectile _Projectile;
    //General Movement has One Animation ---
    [SerializeField] private string Start_Attack_AnimationClipName;
    [SerializeField] private string Launch_Attack_AnimationClipName;
    [SerializeField] private string Finish_Attack_AnimationClipName;
    private bool LaunchAttack;
    Vector2 TargetDirection;

    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        Driver.Get_EnemyRB.velocity = Vector2.zero;

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

        LaunchAttack = false;
    }

    public override void onFixedUpdate()
    {
        if (LaunchAttack)
        {
            //Spawn the Projectile Set the Target Play Particle Effect Etc.
            Vector2 FirePoint = Vector2.zero;
            if (Driver.Modified_FirePoint != null)
                FirePoint = Driver.Modified_FirePoint.position;
            else
                FirePoint = Driver.transform.position;

            var clone = Instantiate(_Projectile, FirePoint, Quaternion.identity);
            clone.Target = PlayerController.Get_Controller.Get_PlayerRB.transform;
            Finish_Attack();
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
