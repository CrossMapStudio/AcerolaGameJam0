using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Hit", menuName = "States/Player/Hit")]
public class PlayerState_Hit : BaseState
{
    [SerializeField] private string HitAnimation_ClipName;
    [SerializeField] private Material Player_Hit;
    private Material Stored_Material;

    #region Camera Channel
    [SerializeField] private float CameraShake_Amplitude, CameraShake_Frequency, CameraShake_Smooth;
    [SerializeField] private CameraShake_Channel CameraShakeChannel;
    #endregion

    //Queue Action for the Player Out of Stun ---
    private UnityEngine.Events.UnityEvent _QueueAction;

    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        _QueueAction = null;

        Stored_Material = PlayerController.Get_Controller.Get_Renderer.material;
        PlayerController.Get_Controller.On_Finish.OnEventRaised.AddListener(Stun_Ended);
        PlayerController.Get_Controller.On_Event.OnEventRaised.AddListener(Flash_End);
        PlayerController.Get_Controller.Get_PlayerAnimator.Play(HitAnimation_ClipName, 0, 0);

        if (Player_Hit != null)
            PlayerController.Get_Controller.Get_Renderer.material = Player_Hit;

        CameraShakeChannel.RaiseEvent(CameraShake_Amplitude, CameraShake_Frequency, CameraShake_Smooth);

        PlayerController.Get_Controller.Get_PlayerRB.velocity = Vector2.zero;
        PlayerController.Get_Controller.Get_PlayerRB.AddForce(PlayerController.Get_Controller.Hit_ForceDirection.normalized * 5f, ForceMode2D.Impulse);

        Player_InputDriver.Get_AttackLight.performed += QueueAttack;
        Player_InputDriver.Get_Dash.performed += QueueDash;
    }

    public override void onExit()
    {
        PlayerController.Get_Controller.On_Finish.OnEventRaised.RemoveListener(Stun_Ended);
        PlayerController.Get_Controller.On_Event.OnEventRaised.RemoveListener(Flash_End);
        Flash_End();

        Player_InputDriver.Get_AttackLight.performed -= QueueAttack;
        Player_InputDriver.Get_Dash.performed -= QueueDash;
    }

    public override void onFixedUpdate()
    {

    }

    public override void onInactiveUpdate()
    {

    }

    public override void onLateUpdate()
    {

    }

    public override void onUpdate()
    {

    }

    public void Flash_End()
    {
        PlayerController.Get_Controller.Get_Renderer.material = Stored_Material;
    }

    public void Stun_Ended()
    {
        //Check for Queue --- Perform
        if (_QueueAction != null && PlayerController.Get_Controller.Get_Health > 0)
            _QueueAction.Invoke();
        else if (PlayerController.Get_Controller.Get_Health <= 0)
        {
            return;
        }
        else
        {
            PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
        }
    }
    private void QueueDash(InputAction.CallbackContext context)
    {
        if (_QueueAction == null)
        {
            _QueueAction = new UnityEngine.Events.UnityEvent();
            _QueueAction.AddListener(Dash_AfterStun);
        }
    }

    private void QueueAttack(InputAction.CallbackContext context)
    {
        if (_QueueAction == null)
        {
            _QueueAction = new UnityEngine.Events.UnityEvent();
            _QueueAction.AddListener(Attack_AfterStun);
        }
    }

    private void Dash_AfterStun()
    {
        //Dash State
        _QueueAction.RemoveAllListeners();
        _QueueAction = null;
        PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[2]);
    }

    private void Attack_AfterStun()
    {
        _QueueAction.RemoveAllListeners();
        _QueueAction = null;
        PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[3]);
    }
}
