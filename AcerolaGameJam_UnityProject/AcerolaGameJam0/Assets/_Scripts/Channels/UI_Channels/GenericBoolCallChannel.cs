using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Call Bool Channel", menuName = "Channels/Bool Call Channel")]
public class GenericBoolCallChannel : ScriptableObject
{
    public UnityEvent<bool> OnEventRaised;
    public void RaiseEvent(bool _Trigger)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(_Trigger);
    }
}

