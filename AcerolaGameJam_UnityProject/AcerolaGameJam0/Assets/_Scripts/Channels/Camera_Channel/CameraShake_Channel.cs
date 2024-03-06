using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Camera Shake Channel", menuName = "Channels/Camera Shake")]
public class CameraShake_Channel : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent<float, float, float> OnEventRaised;
    public void RaiseEvent(float _mag, float _frq, float _damp)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(_mag, _frq, _damp);
    }
}
