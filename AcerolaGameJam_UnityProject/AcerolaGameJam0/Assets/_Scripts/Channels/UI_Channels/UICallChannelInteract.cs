using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UI Call Channel", menuName = "Channels/UI Call Channel Interact")]
public class UICallChannelInteract : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent<UIInteractData> OnEventRaised;
    public void RaiseEvent(UIInteractData _data)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(_data);
    }
}
