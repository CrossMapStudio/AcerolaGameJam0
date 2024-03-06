using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Melee", menuName = "Weapon Types/Melee")]
public class Melee : ScriptableObject
{
    //Better Here Since Only Melee Will Need This ---
    public Combat_Channel CombatChannel;
    //Weapon Statistics for Melee ---
    public int Attack_CombinationLimit;
    public int damageValue;

    private UnityEvent On_AttackFinish;
    public UnityEvent Get_OnAttackFinish => On_AttackFinish;

    public void OnEnable()
    {
        //Stupid Ahh Problem Forced me to Delete a Whole Abstract Structure this BS
        On_AttackFinish = new UnityEvent();
    }

    public void OnLightAttack_Initialized()
    {
        //Nothing Here ---
        Debug.Log("Attack Initialized");
    }

    public void OnLightAttack_AnimationEvent()
    {
        CombatChannel.RaiseEvent(Player_InputDriver.Get_StoredDirection, damageValue);
    }

    public void OnLightAttack_Finished()
    {
        if (Get_OnAttackFinish != null)
            Get_OnAttackFinish.Invoke();
    }
}