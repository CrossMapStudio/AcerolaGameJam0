using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Attack", menuName = "States/Player/Player Attack")]
public class PlayerState_Attack : BaseState
{
    public int comboCountLimit = 0;
    //Used for Shifting the Player On Attack ---
    Vector2 TargetMovePoint;
    public float StepDistance = 1.8f;
    public float MovementInfluence = .35f;
    private UnityEngine.Events.UnityEvent QueueAction;

    //Weapon Base ---
    private Collider2D HitRadius;

    //Melee ->
    //[SerializeField] private Combat_Channel Combat_Channel;

    //Weapon Type
    [SerializeField] private Melee Assigned_WeaponType;
    [SerializeField] private CombatInvoke On_LightInitialized, On_LightEvent, On_LightFinished;


    public override bool checkValid()
    {
        On_LightInitialized.OnEventRaised.RemoveAllListeners();
        On_LightEvent.OnEventRaised.RemoveAllListeners();
        On_LightFinished.OnEventRaised.RemoveAllListeners();

        Debug.Log("Weapon Type: " + Assigned_WeaponType);

        Assigned_WeaponType.Get_OnAttackFinish.AddListener(On_AttackFinished);

        On_LightInitialized.OnEventRaised.AddListener(Assigned_WeaponType.OnLightAttack_Initialized);
        On_LightEvent.OnEventRaised.AddListener(Assigned_WeaponType.OnLightAttack_AnimationEvent);
        On_LightFinished.OnEventRaised.AddListener(Assigned_WeaponType.OnLightAttack_Finished);
        PlayerController.Get_Controller.GetAttack_Values.comboCount = 0;
        return true;
    }

    public override void onEnter()
    {
        HitRadius = PlayerController.Get_Controller.HitRadius;
        TargetMovePoint = Player_InputDriver.Get_StoredDirection;
        TargetMovePoint *= StepDistance;

        Player_InputDriver.Get_AttackLight.performed += QueueAttack;
        Player_InputDriver.Get_Dash.performed += Dash;
        QueueAction = null;
        PerformAttack();
    }

    public override void onExit()
    {
        Player_InputDriver.Get_Dash.performed -= Dash;
        Player_InputDriver.Get_AttackLight.performed -= QueueAttack;
        Assigned_WeaponType.Get_OnAttackFinish.RemoveListener(On_AttackFinished);
    }
    public override void onUpdate()
    {
        TargetMovePoint = Vector3.Lerp(TargetMovePoint, Vector2.zero, Time.deltaTime * 20f);
    }

    public override void onInactiveUpdate()
    {

    }

    public override void onFixedUpdate()
    {
        PlayerController.Get_Controller.Get_PlayerRB.MovePosition(PlayerController.Get_Controller.Get_PlayerRB.position + (TargetMovePoint + (Player_InputDriver.Get_StoredDirection * MovementInfluence)) * Time.deltaTime);
    }

    public override void onLateUpdate()
    {

    }
    private void QueueAttack(InputAction.CallbackContext context)
    {
        if (QueueAction == null)
        {
            QueueAction = new UnityEngine.Events.UnityEvent();
            PlayerController.Get_Controller.GetAttack_Values.comboCount++;
            QueueAction.AddListener(PerformAttack);
        }
    }
    private void On_AttackFinished()
    {
        if (QueueAction != null)
        {
            QueueAction.Invoke();
            QueueAction.RemoveAllListeners();
            QueueAction = null;
        }
        else
        {
            PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
        }
    }
    private void PerformAttack()
    {
        if (PlayerController.Get_Controller.GetAttack_Values.comboCount < comboCountLimit)
        {
            TargetMovePoint = Player_InputDriver.Get_StoredDirection;
            TargetMovePoint *= StepDistance;

            PlayerController.Get_Controller.Get_Renderer.flipX = true ? Player_InputDriver.Get_StoredDirection.x < 0 : false;
            PlayerController.Get_Controller.Get_PlayerAnimator.Play("Light_Attack" + PlayerController.Get_Controller.GetAttack_Values.comboCount.ToString(), 0, 0);
        }
        else
        {
            PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[2]);
    }
}
