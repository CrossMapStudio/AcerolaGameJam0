using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageBox : Interactable
{
    public ReadMessage_SO Message_Data;
    public Animator Box_Animator;
    public TMP_Text Message_Text;

    private bool PlayExit = false;
    
    public UIInteractData Interact_Instructions;

    protected virtual void Awake()
    {
        base.Awake();
        Message_Text.text = Message_Data.Get_Message;
    }

    protected override void On_Enter()
    {
        //Do Nothing
        Interact_Call.RaiseEvent(Interact_Instructions);
    }

    protected override void On_Exit()
    {
        if (PlayExit)
            Box_Animator.Play("MessageBoxHideMessage");

        Interact_Call.RaiseEvent(Interact_Instructions);
    }

    protected override void On_Interact()
    {
        Box_Animator.Play("MessageBoxShowMessage");
        PlayExit = true;
    }
}
