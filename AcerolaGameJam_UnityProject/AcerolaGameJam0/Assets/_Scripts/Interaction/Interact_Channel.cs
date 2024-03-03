using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Interaction Channel", menuName = "Channels/Interact")]
public class Interact_Channel : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent OnEventRaised;
    public void RaiseEvent(InputAction.CallbackContext context)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke();
    }
}
