using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Free Movement", menuName = "States/Player/Free Movement")]
public class PlayerState_FreeMovement : BaseState
{
    [SerializeField] private float Player_Speed;
    private Vector2 Input, StoredMovement;

    //Used for Interaction ---
    [SerializeField] private Interact_Channel Interact;

    //Collect Active Interaction Objects and Set Them ---
    private InteractionBase Active_InteractionBase;

    public override void onEnter()
    {
        //Start Idle Animation ---
        Player_InputDriver.Get_Dash.performed += Dash;
        Player_InputDriver.Get_AttackLight.performed += Attack;
        Player_InputDriver.Get_Interact.performed += Interact.RaiseEvent;

        //Reset
        Active_InteractionBase = null;
    }

    public override void onExit()
    {
        Player_InputDriver.Get_Dash.performed -= Dash;
        Player_InputDriver.Get_AttackLight.performed -= Attack;
        Player_InputDriver.Get_Interact.performed -= Interact.RaiseEvent;

        On_InteractionSetExit();
    }

    public override void onFixedUpdate()
    {
        PlayerController.Get_Controller.Get_PlayerRB.MovePosition(PlayerController.Get_Controller.Get_PlayerRB.position + (Input * Player_Speed * Time.deltaTime));
        On_InteractionSet(PlayerInteractionBase.CallInteractionCheck());
    }

    public override void onLateUpdate()
    {

    }

    public override void onUpdate()
    {
        Input = Player_InputDriver.Get_Movement.ReadValue<Vector2>();

        if (Input == Vector2.zero)
        {
            PlayerController.Get_Controller.Get_PlayerAnimator.Play("Player_Idle");
        }
        else
        {
            PlayerController.Get_Controller.Get_PlayerAnimator.Play("Player_Run");
        }

        if (Input.x != 0)
        {
            StoredMovement.x = Input.x;
        }

        PlayerController.Get_Controller.Get_Renderer.flipX = true ? StoredMovement.x < 0 : false;
    }

    public override void onInactiveUpdate()
    {
        //
    }

    public override bool checkValid()
    {
        return true;
    }

    //Assigned State Changes ---
    public void Dash(InputAction.CallbackContext context)
    {
        PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[2]);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[3]);
    }

    public void On_InteractionSet(InteractionBase _Base)
    {
        if (_Base == null && Active_InteractionBase != null)
        {
            Active_InteractionBase.DisableInteraction();
            Active_InteractionBase = null;
            return;
        }

        if (Active_InteractionBase != _Base)
        {
            if (Active_InteractionBase != null)
                Active_InteractionBase.DisableInteraction();
            Active_InteractionBase = _Base;
            Active_InteractionBase.EnableInteraction();
        }
    }

    public void On_InteractionSetExit()
    {
        if (Active_InteractionBase != null)
        {
            Active_InteractionBase.DisableInteraction();
            Active_InteractionBase = null;
            return;
        }
    }
}
