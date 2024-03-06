using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Camera Target Animation Channel", menuName = "Channels/Camera Animation Target")]
public class CameraTargetChannel : ScriptableObject
{
    public UnityEvent<Cinemachine.CinemachineVirtualCamera> OnEventRaised_Enter;
    public UnityEvent<Cinemachine.CinemachineVirtualCamera> OnEventRaised_Exit;
    public void RaiseEvent_OnEnter(Cinemachine.CinemachineVirtualCamera Target)
    {
        if (OnEventRaised_Enter != null)
            OnEventRaised_Enter.Invoke(Target);
    }

    public void RaiseEvent_OnExit(Cinemachine.CinemachineVirtualCamera Target)
    {
        if (OnEventRaised_Exit != null)
            OnEventRaised_Exit.Invoke(Target);
    }
}

