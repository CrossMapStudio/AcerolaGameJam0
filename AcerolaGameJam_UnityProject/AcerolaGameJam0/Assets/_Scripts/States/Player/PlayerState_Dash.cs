using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Dash", menuName = "States/Player/Player Dash")]
public class PlayerState_Dash : BaseState
{
    private Vector2 Dash_Direction;
    [Range(0f, 100f)]
    [SerializeField] private float Dash_Speed;
    [SerializeField] private float Dash_Distance;
    [SerializeField] private int Dash_AmountTarget;

    [SerializeField] private float Dash_RecoveryTarget;
    [SerializeField] private float Dash_TimeTarget;

    private Vector2 Start_Point;
    private Vector2 End_Point;

    [SerializeField] private LayerMask Raycast_HitLayers;

    //Queue Dash ---
    private UnityEvent<Vector2> ExecuteDashOnQueue = null;
    private Vector2 _QueueDirection;

    public override void onEnter()
    {
        ExecuteDash(Vector2.zero);
        Player_InputDriver.Get_Dash.performed += QueueDash;
    }

    public override void onExit()
    {
        PlayerController.Get_Controller.DashParticles.Stop();
        Player_InputDriver.Get_Dash.performed -= QueueDash;
    }

    public override void onFixedUpdate()
    {
        PlayerController.Get_Controller.Get_PlayerRB.MovePosition(Start_Point);
    }

    public override void onLateUpdate()
    {

    }

    public override void onUpdate()
    {
        Start_Point = Vector2.Lerp(Start_Point, End_Point, Time.deltaTime * Dash_Speed);
        if (PlayerController.Get_Controller.Get_DashValues.Current_DashTime < Dash_TimeTarget)
        {
            PlayerController.Get_Controller.Get_DashValues.Current_DashTime += Time.deltaTime;
        }
        else
        {
            //Check if Queue ---
            if (ExecuteDashOnQueue != null)
            {
                ExecuteDashOnQueue.Invoke(_QueueDirection);
                ExecuteDashOnQueue.RemoveListener(ExecuteDash);
                ExecuteDashOnQueue = null;
            }
            else
                PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
        }
    }

    public override void onInactiveUpdate()
    {
        if (PlayerController.Get_Controller.Get_DashValues.Current_DashAmount > 0)
        {
            PlayerController.Get_Controller.Get_DashValues.Current_DashRecovery += Time.deltaTime;
            if (PlayerController.Get_Controller.Get_DashValues.Current_DashRecovery >= Dash_RecoveryTarget)
            {
                PlayerController.Get_Controller.Get_DashValues.Current_DashRecovery = 0f;
                PlayerController.Get_Controller.Get_DashValues.Current_DashAmount--;
            }
        }
    }

    public override bool checkValid()
    {
        if (PlayerController.Get_Controller.Get_DashValues.Current_DashAmount < Dash_AmountTarget) return true; else return false;
    }

    public void ExecuteDash(Vector2 QueueDirection)
    {
        Vector2 Direction = Vector2.zero;
        if (QueueDirection == Vector2.zero)
            Direction = Player_InputDriver.Get_Movement.ReadValue<Vector2>();
        else
            Direction = QueueDirection;

        if (Direction == Vector2.zero)
            PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
        else
        {
            Dash_Direction = Direction;

            End_Point = PlayerController.Get_Controller.Get_PlayerRB.position + (Dash_Direction * Dash_Distance);
            Start_Point = PlayerController.Get_Controller.Get_PlayerRB.position;
            PlayerController.Get_Controller.Get_DashValues.Current_DashAmount++;

            PlayerController.Get_Controller.DashParticles.Play();
            PlayerController.Get_Controller.Get_PlayerAnimator.Play("Player_Dash");

            PlayerController.Get_Controller.Get_DashValues.Current_DashTime = 0f;
        }
    }

    public void QueueDash(InputAction.CallbackContext context)
    {
        if (PlayerController.Get_Controller.Get_DashValues.Current_DashAmount < Dash_AmountTarget)
        {
            ExecuteDashOnQueue = new UnityEvent<Vector2>();
            _QueueDirection = Player_InputDriver.Get_Movement.ReadValue<Vector2>();
            ExecuteDashOnQueue.AddListener(ExecuteDash);
        }
    }
}
