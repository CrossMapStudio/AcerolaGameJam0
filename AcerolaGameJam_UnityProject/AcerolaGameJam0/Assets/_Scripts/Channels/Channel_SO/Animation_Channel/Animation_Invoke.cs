using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Animation Invoke", menuName = "Animation Invoke/Animation Caller")]
public class Animation_Invoke : ScriptableObject
{
    public UnityEvent OnEventRaised;
    public void RaiseEvent()
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke();
    }
}
