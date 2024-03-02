using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region State Machines
public class StateMachine
{
    private BaseState currentState, previousState;
    public void changeState(BaseState newState, GameObject stateIdentity = null)
    {
        if (!newState.checkValid())
            return;

        if (currentState != null)
        {
            this.currentState.onExit();
        }
        this.previousState = this.currentState;
        this.currentState = newState;
        this.currentState.onEnter();
    }

    public void executeStateUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            this.currentState.onUpdate();
        }
    }

    public void executeStateFixedUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            this.currentState.onFixedUpdate();
        }
    }

    public void executeStateLateUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            this.currentState.onLateUpdate();
        }
    }

    public void previousStateSwitch()
    {
        if (this.previousState != null)
        {
            this.currentState.onExit();
            this.currentState = this.previousState;
            this.currentState.onEnter();
        }
        else
        {
            return;
        }
    }

    //To Allow Us to Check for the State
    public BaseState getCurrentState()
    {
        return currentState;
    }

    public string getCurrentStateName()
    {
        return currentState.ID;
    }
}
#endregion
