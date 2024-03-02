using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Free Movement", menuName = "States/Player/Free Movement")]
public class PlayerState_FreeMovement : BaseState
{
    [SerializeField] private float Player_Speed;
    private Vector2 Input, StoredMovement;

    public override void onEnter()
    {
        //Start Idle Animation ---
        Player_InputDriver.Get_Dash.performed += Dash;
    }

    public override void onExit()
    {
        Player_InputDriver.Get_Dash.performed -= Dash;
    }

    public override void onFixedUpdate()
    {
        PlayerController.Get_Controller.Get_PlayerRB.MovePosition(PlayerController.Get_Controller.Get_PlayerRB.position + (Input * Player_Speed * Time.deltaTime));
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
}
