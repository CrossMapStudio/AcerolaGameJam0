using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Needs Path Finding ---
[CreateAssetMenu(fileName = "Basic Enemy Tracking State", menuName = "Enemy State/Tracking Enemy State")]
public class Enemy_Tracking : Enemy_BaseState
{
    public Enemy_Tracking()
    {

    }
    
    public override bool checkValid()
    {
        return true;
    }

    public override void onEnter()
    {

    }

    public override void onExit()
    {
      
    }

    public override void onFixedUpdate()
    {
        Vector3 direction = (Brain.Get_Driver.Get_Target.position - Brain.Get_Driver.Get_Position).normalized;
        Brain.Get_Driver.Get_EnemyRB.MovePosition(Brain.Get_Driver.Get_EnemyRB.transform.position + (direction * 1.5f * Time.deltaTime));
    }

    public override void onInactiveUpdate()
    {
  
    }

    public override void onLateUpdate()
    {
    
    }

    public override void onUpdate()
    {

    }
}
