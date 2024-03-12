using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Dead", menuName = "States/Player/Dead")]
public class PlayerState_Dead : BaseState
{
    //Play the Animation ---
    //On the Finish Animation Event Call the UI Close Channel
    //UI Finish Close --- Call Game Manager Respawn
    //Get Designated Respawn Point ---
    //Move the Player to the Respawn Point ---
    //Refill the Level
    //Set the Player Stats Back to Normal
    //Open UI Call
    //Intro Spawn Sequence
    //Play

    [SerializeField] private string DeathAnimation_ClipName;
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {
        PlayerController.Get_Controller.Get_PlayerAnimator.Play(DeathAnimation_ClipName, 0, 0);
        PlayerController.Get_Controller.On_Finish.OnEventRaised.AddListener(Call_UIClose);

        PlayerController.Get_Controller.Get_PlayerRB.velocity = Vector2.zero;
    }

    public override void onExit()
    {
        PlayerController.Get_Controller.On_Finish.OnEventRaised.RemoveListener(Call_UIClose);
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

    public void Call_UIClose()
    {
        //Close the UI --- Wait for Input to Respawn ---
    }
}
