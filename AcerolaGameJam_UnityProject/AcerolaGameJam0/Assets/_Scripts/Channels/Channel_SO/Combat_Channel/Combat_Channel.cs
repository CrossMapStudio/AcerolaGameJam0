using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Combat Channel", menuName = "Channels/Combat Channel")]
public class Combat_Channel : ScriptableObject
{
    //Might Need to Change for Other Modifiers??? ---- For Now this will work ---
    public UnityEvent<Vector2, float> OnEventRaised;
    public void RaiseEvent(Vector2 direction, float damage)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(direction, damage);
    }
}
