using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Heal", menuName = "States/Player/Heal")]
public class PlayerState_Heal : BaseState
{
    [SerializeField] private string Heal_AnimationClipName;
    [SerializeField] private Material Heal_GlowMaterial;
    private Material Stored_Mat;


    public override bool checkValid()
    {
        if (PlayerController.Get_Controller.Current_MedicalCount > 0)
            return true;
        else
        {
            PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
            return false;
        }
    }

    public override void onEnter()
    {
        Stored_Mat = PlayerController.Get_Controller.Get_Renderer.material;
        PlayerController.Get_Controller.On_Event.OnEventRaised.AddListener(Heal_Player);
        PlayerController.Get_Controller.On_Finish.OnEventRaised.AddListener(Heal_Finished);

        PlayerController.Get_Controller.Player_CombatChannel.OnEventRaised.AddListener(PlayerController.Get_Controller.Take_Damage);

        PlayerController.Get_Controller.Get_PlayerRB.velocity = Vector2.zero;
        PlayerController.Get_Controller.Get_PlayerAnimator.Play(Heal_AnimationClipName, 0, 0);
    }

    public override void onExit()
    {
        PlayerController.Get_Controller.On_Event.OnEventRaised.RemoveListener(Heal_Player);
        PlayerController.Get_Controller.On_Finish.OnEventRaised.RemoveListener(Heal_Finished);

        PlayerController.Get_Controller.Get_Renderer.material = Stored_Mat;

        PlayerController.Get_Controller.Player_CombatChannel.OnEventRaised.RemoveListener(PlayerController.Get_Controller.Take_Damage);
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
    private void Heal_Player()
    {
        PlayerController.Get_Controller.Current_MedicalCount--;
        PlayerController.Get_Controller.Heal_Player();
        if (Heal_GlowMaterial != null)
            PlayerController.Get_Controller.Get_Renderer.material = Heal_GlowMaterial;
    }

    private void Heal_Finished()
    {
        PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
    }
}
