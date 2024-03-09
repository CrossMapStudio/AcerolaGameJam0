using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Basic", menuName = "Enemy Brain/Enemy Basic")]
public class Enemy_Basic : Enemy_Brain
{
    public override void SetTarget(Transform _Target)
    {
        Get_EnemyStateMachine.changeState(CreateInstance<Enemy_Tracking>(), this);
    }
}
