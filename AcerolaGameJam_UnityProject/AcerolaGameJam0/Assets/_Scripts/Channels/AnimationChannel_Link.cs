using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChannel_Link : MonoBehaviour
{
    public Animation_Invoke On_Init;
    public Animation_Invoke On_Event;
    public Animation_Invoke On_Finish;

    public void On_AnimationStart()
    {
        On_Init.OnEventRaised.Invoke();
    }

    public void On_AnimationEvent()
    {
        On_Event.OnEventRaised.Invoke();
    }

    public void On_AnimationFinished()
    {
        On_Finish.OnEventRaised.Invoke();
    }
}
