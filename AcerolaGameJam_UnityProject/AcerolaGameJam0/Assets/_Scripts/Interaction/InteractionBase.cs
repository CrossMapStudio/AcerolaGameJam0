using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionBase : MonoBehaviour
{
    private UnityAction Subscriber;
    private UnityAction OnEnter;
    private UnityAction OnExit;
    public UnityAction GetSet_Subscriber { get => Subscriber; set => Subscriber = value; }
    public UnityAction GetSet_OnEnter { get => OnEnter; set => OnEnter = value; }
    public UnityAction GetSet_OnExit { get => OnExit; set => OnExit = value; }
    public TutorialData Get_TutorialData { get => Tutorial_Data; }
    public TutorialUI_Channel Get_TutorialChannel { get => Tutorial_Channel; set => Tutorial_Channel = value; }

    [SerializeField] private Interact_Channel Channel;

    //Tutorials
    [SerializeField] private TutorialData Tutorial_Data;
    [SerializeField] private TutorialUI_Channel Tutorial_Channel;

    public void EnableInteraction()
    {
        Channel.OnEventRaised.AddListener(Subscriber);
        if (OnEnter != null)
        {
            OnEnter.Invoke();
        }
    }

    public void DisableInteraction()
    {
        Channel.OnEventRaised.RemoveListener(Subscriber);
        if (OnExit != null)
        {
            OnExit.Invoke();
        }
    }

    public void OnApplicationQuit()
    {
        Channel.OnEventRaised.RemoveListener(Subscriber);
    }
}
