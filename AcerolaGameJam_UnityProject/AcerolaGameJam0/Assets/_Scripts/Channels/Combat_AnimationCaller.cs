using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_AnimationCaller : MonoBehaviour
{
    public CombatInvoke On_LightInitialized;
    public CombatInvoke On_LightEvent;
    public CombatInvoke On_LightFinished;

    public void On_LightAttackInitialize()
    {
        On_LightInitialized.OnEventRaised.Invoke();
    }

    public void On_LightAttackEvent()
    {
        On_LightEvent.OnEventRaised.Invoke();
    }

    public void On_LightAttackFinished()
    {
        On_LightFinished.OnEventRaised.Invoke();
    }
}
