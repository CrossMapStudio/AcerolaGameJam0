using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UI Call Channel", menuName = "Channels/UI Call Channel Float")]
public class UICallChannelFloat : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent<float> OnEventRaised;
    public void RaiseEvent(float Value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(Value);
    }
}