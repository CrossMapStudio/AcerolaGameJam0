using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Call Channel", menuName = "Channels/Generic Call Channel")]
public class GenericCallChannel : ScriptableObject
{
    public UnityEvent OnEventRaised;
    public void RaiseEvent()
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke();
    }
}
