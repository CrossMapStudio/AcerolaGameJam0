using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Combat Invoke", menuName = "Combat Invoke/Animation Caller")]
public class CombatInvoke : ScriptableObject
{
    public UnityEvent OnEventRaised;
    public void RaiseEvent()
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke();
    }
}
