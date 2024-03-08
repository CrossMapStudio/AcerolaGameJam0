using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortionInteract : Interactable
{
    [SerializeField] private GenericCallChannel OnFixCallLink;

    public Animator DistortionAnimator;
    public UIInteractData Interact_Instructions;
    public Collider2D InteractionCollider;

    [SerializeField] private SpriteRenderer _Renderer;
    [SerializeField] private Material FixedMaterial;

    protected virtual void Awake()
    {
        base.Awake();
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

        if (PlayerController.Get_Controller.GetSet_CurrentShard != null)
        {
            if (DistortionAnimator != null)
                DistortionAnimator.Play("FixDistortion");


            InteractionCollider.enabled = false;
            OnFixCallLink.RaiseEvent();

            _Renderer.material = FixedMaterial;

            //Grab the Current Shard --- Play Animation ---
            Destroy(PlayerController.Get_Controller.GetSet_CurrentShard.gameObject);
            PlayerController.Get_Controller.GetSet_CurrentShard = null;
        }
    }
}
