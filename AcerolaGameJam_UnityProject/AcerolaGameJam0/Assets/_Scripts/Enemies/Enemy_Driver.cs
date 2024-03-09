using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy_Driver : MonoBehaviour
{
    [SerializeField] private float Starting_Health;
    [SerializeField] private Enemy_Brain Set_InspectorBrain;
    private Enemy_Brain Brain;
    //Collect Data within the Enemy Driver and The Brain will Utilize it to Produce Enemy Actions ---

    //Player Detection
    [SerializeField] private bool _DetectPlayerImmediate;
    
    private Transform Target;
    public Transform Get_Target => Target;

    [SerializeField] private float Detection_Radius;
    [SerializeField] private LayerMask Detection_Layers;

    //Driver Actions that the Brain Listens too ---
    private Action<Transform> Call_SetTarget;
    public Action<Transform> GetSet_CallTarget { get => Call_SetTarget; set => Call_SetTarget = value; }

    //Rigidbody
    private Rigidbody2D Enemy_RB;
    public Rigidbody2D Get_EnemyRB => Enemy_RB;

    public Vector3 Get_Position => transform.position;

    public void Awake()
    {
        Brain = ScriptableObject.CreateInstance<Enemy_Basic>();

        Enemy_RB = GetComponent<Rigidbody2D>();

        GetSet_CallTarget += SetTarget; 

        Brain.Init_Enemy(this);
        if (_DetectPlayerImmediate)
            Call_SetTarget(PlayerController.Get_Controller.Get_PlayerRB.transform);
    }

    public void SetTarget(Transform _Target)
    {
        Target = _Target;
    }

    public void Update()
    {
        Brain.Update();
    }

    public void FixedUpdate()
    {
        Brain.FixedUpdate();

        if (Target == null)
        {
            Collider2D Player_Collider = Physics2D.OverlapCircle(transform.position, Detection_Radius, Detection_Layers);
            //Detect Closest Target --- Set For Duration, Then Check Again??? --- For now just detect the Player
            if (Player_Collider != null)
                Call_SetTarget(PlayerController.Get_Controller.Get_PlayerRB.transform);
        }
    }

    public void LateUpdate()
    {
        Brain.LateUpdate();
    }

    //Info to Collect --- Target Player Position, If Player Detected, If Detection Needed --- Taking Damage, Health <= 0,  
}
