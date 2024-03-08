using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Tutorial Channel", menuName = "Channels/Tutorial")]
public class TutorialUI_Channel : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent<TutorialData> OnEventRaised;
    public void RaiseEvent_ShowTutorial(TutorialData data)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(data);
    }
}