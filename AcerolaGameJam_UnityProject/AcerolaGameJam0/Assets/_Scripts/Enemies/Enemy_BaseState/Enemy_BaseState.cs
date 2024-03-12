using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_BaseState : ScriptableObject
{
    public string ID;
    [HideInInspector]
    public Enemy_Driver Driver;

    public abstract void onEnter();
    public abstract void onUpdate();
    public abstract void onFixedUpdate();
    public abstract void onLateUpdate();
    public abstract void onExit();
    public abstract void onInactiveUpdate();
    public abstract bool checkValid();
}
