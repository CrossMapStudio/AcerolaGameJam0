using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SingletonPersistent<PlayerController> 
{
    //Singleton Instance and the Controller will Always be Set to this in Awake ---
    private static PlayerController Controller;
    public static PlayerController Get_Controller => Controller; 
    
    private StateMachine Player_StateMachine;
    public StateMachine Get_StateMachine => Player_StateMachine;

    [SerializeField] private List<BaseState> States;
    public List<BaseState> Get_States => States;

    //Used for the Player Movement ---
    private Rigidbody2D Player_RB;
    public Rigidbody2D Get_PlayerRB => Player_RB;

    //Renderer For Flipping
    [SerializeField] private SpriteRenderer Renderer;
    public SpriteRenderer Get_Renderer => Renderer;

    //Animator
    [SerializeField] private Animator Player_Animator;
    public Animator Get_PlayerAnimator => Player_Animator;

    private Dash_Values DashValues;
    public Dash_Values Get_DashValues => DashValues;

    //Particles for Dash
    public ParticleSystem DashParticles;

    protected override void Awake()
    {
        Controller = this;

        Player_StateMachine = new StateMachine();
        Player_RB = GetComponent<Rigidbody2D>();

        DashValues = new Dash_Values();
    }

    public void Start()
    {
        Player_StateMachine.changeState(States[1]);
    }

    // Update is called once per frame
    private void Update()
    {
        //Input Actions to Change States ---
        Player_StateMachine.executeStateUpdate();

        foreach(BaseState element in States)
        {
            element.onInactiveUpdate();
        }
    }

    private void FixedUpdate()
    {
        Player_StateMachine.executeStateFixedUpdate();
    }

    private void LateUpdate()
    {
        Player_StateMachine.executeStateLateUpdate();
    }

    private void Change_PlayerState(int StateIndex)
    {
        Player_StateMachine.changeState(States[StateIndex]);
    }
}

public class Dash_Values
{
    public float Current_DashRecovery = 0f;
    public float Current_DashTime = 0f;
    public int Current_DashAmount = 0;
}
