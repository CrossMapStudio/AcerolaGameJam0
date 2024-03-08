using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shards_Interaction : Interactable
{
    public Animator Shard_Animator;
    private bool PlayExit = false;

    public UIInteractData Interact_Instructions;

    public Vector2 FollowOffset;
    public float SmoothSpeed;
    private Transform Target;

    public Collider2D InteractionCollider;

    protected virtual void Awake()
    {
        base.Awake();
    }

    public void LateUpdate()
    {
        //If a shard is collected it will set target and follow the player --- if the player dies we need to reset the shard???
        //If the shard is used we need to have a caller and animation for it ---
        if (Target != null)
        {
            transform.position = Vector2.Lerp(transform.position, (Vector2)Target.position + FollowOffset, Time.deltaTime * SmoothSpeed);
        }
    }

    protected override void On_Enter()
    {
        //Do Nothing
        Interact_Call.RaiseEvent(Interact_Instructions);
    }

    protected override void On_Exit()
    {
        Interact_Call.RaiseEvent(Interact_Instructions);
    }

    protected override void On_Interact()
    {
        if (Get_InteractionBase.Get_TutorialChannel != null)
        {
            Get_InteractionBase.Get_TutorialChannel.RaiseEvent_ShowTutorial(Get_InteractionBase.Get_TutorialData);
            Get_InteractionBase.Get_TutorialChannel = null;
            return;
        }

        if (PlayerController.Get_Controller.GetSet_CurrentShard == null)
        {
            Shard_Animator.Play("TimeShardCollect");
            //Add Shard to Player Count ---
            PlayerController.Get_Controller.GetSet_CurrentShard = this;
            Target = PlayerController.Get_Controller.Get_PlayerRB.transform;
            InteractionCollider.enabled = false;
        }
        else
        {
            //Reject ---
        }
    }
}
