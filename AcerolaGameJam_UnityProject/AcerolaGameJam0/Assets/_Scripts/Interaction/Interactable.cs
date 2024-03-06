using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionBase))]
public abstract class Interactable : MonoBehaviour
{
    private InteractionBase Interaction_Base;
    public InteractionBase Get_InteractionBase => Interaction_Base;

    public UICallChannelInteract Interact_Call;

    protected void Awake()
    {
        Interaction_Base = GetComponent<InteractionBase>();
        Interaction_Base.GetSet_OnEnter += On_Enter;
        Interaction_Base.GetSet_OnExit += On_Exit;
        Interaction_Base.GetSet_Subscriber += On_Interact;
    }

    protected void OnApplicationQuit()
    {
        Interaction_Base.GetSet_OnEnter -= On_Enter;
        Interaction_Base.GetSet_OnExit -= On_Exit;
        Interaction_Base.GetSet_Subscriber -= On_Interact;
    }

    protected abstract void On_Enter();
    protected abstract void On_Exit();
    protected abstract void On_Interact();
}
