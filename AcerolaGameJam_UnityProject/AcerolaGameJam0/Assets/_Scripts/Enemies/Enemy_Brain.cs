using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Brain : ScriptableObject
{
    private Enemy_Driver Driver;
    public Enemy_Driver Get_Driver => Driver;

    private Enemy_StateMachine EnemyStateMachine;
    public Enemy_StateMachine Get_EnemyStateMachine => EnemyStateMachine;
    //Define all updates --- Callbacks --- State Lists --- Etc. Use the data from the Driver to Control the Brain ---
    [SerializeField] private List<Enemy_BaseState> Enemy_States;
    public List<Enemy_BaseState> Get_EnemyStates => Enemy_States;

    public void Init_Enemy(Enemy_Driver _Driver)
    {
        EnemyStateMachine = new Enemy_StateMachine();
        EnemyStateMachine.changeState(CreateInstance<Enemy_Idle>(), this);

        Driver = _Driver;
        Driver.GetSet_CallTarget += SetTarget;
    }

    public virtual void Update()
    {
        EnemyStateMachine.executeStateUpdate();
    }

    public virtual void FixedUpdate()
    {
        EnemyStateMachine.executeStateFixedUpdate();
    }

    public virtual void LateUpdate()
    {
        EnemyStateMachine.executeStateLateUpdate();
    }

    public abstract void SetTarget(Transform _Target);
}
