using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UI Call Channel", menuName = "Channels/UI Call Channel String")]
public class UICallChannelString : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent<string> OnEventRaised;
    public void RaiseEvent(string _data)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(_data);
    }
}

