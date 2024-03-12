using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Guid Channel", menuName = "Channels/Guid Channel")]
public class Guid_Channel : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent<Guid> OnEventRaised;
    public void RaiseEvent(Guid ID)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(ID);
    }
}
