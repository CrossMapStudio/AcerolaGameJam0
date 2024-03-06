using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UI Call Channel", menuName = "Channels/UI Call Channel")]
public class UICallChannel : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent OnEventRaised;
    public void RaiseEvent()
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke();
    }
}
