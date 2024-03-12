using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    private Attack_Values AttackValues;
    public Attack_Values GetAttack_Values => AttackValues;

    //Particles for Dash
    public ParticleSystem DashParticles;

    public Collider2D HitRadius;

    //For Interaction ---
    public float Interaction_Distance;
    public LayerMask Interaction_Layer;

    
    
    //Player Health --- Used for Calling Death State
    private float Health = 100f;
    public float Get_Health => Health;
    public Combat_Channel Player_CombatChannel;
    public UICallChannelFloat HealthBarUpdate;

    //Animation Based Channels ---

    //Player Shard Collection ---
    private Shards_Interaction Current_Shard;
    public Shards_Interaction GetSet_CurrentShard { get => Current_Shard; set => Current_Shard = value; }

    #region Animation Links
    [SerializeField] private AnimationChannel_Link AnimatorLinkChannels;
    [HideInInspector]
    public Animation_Invoke On_Init, On_Event, On_Finish;
    #endregion

    //Used in the Hit State for Direction to Add Force ---
    [HideInInspector] public Vector2 Hit_ForceDirection;

    protected override void Awake()
    {
        base.Awake();
        Controller = this;

        Player_StateMachine = new StateMachine();
        Player_RB = GetComponent<Rigidbody2D>();

        DashValues = new Dash_Values();
        AttackValues = new Attack_Values();

        AnimatorLinkChannels.On_Init = Instantiate(AnimatorLinkChannels.On_Init);
        On_Init = AnimatorLinkChannels.On_Init;

        AnimatorLinkChannels.On_Event = Instantiate(AnimatorLinkChannels.On_Event);
        On_Event = AnimatorLinkChannels.On_Event;

        AnimatorLinkChannels.On_Finish = Instantiate(AnimatorLinkChannels.On_Finish);
        On_Finish = AnimatorLinkChannels.On_Finish;
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

    public void Reset_Player()
    {
        Health = 100f;
        DashValues.ResetValues();
        AttackValues.ResetValues();

        HealthBarUpdate.RaiseEvent(Health);
        Player_StateMachine.changeState(States[1]);
    }

    public void Take_Damage(Vector2 direction, float damage)
    {
        Hit_ForceDirection = direction;
        Health -= damage;
        HealthBarUpdate.RaiseEvent(Health);
        Debug.Log("Damage: " + damage);
        if (Health <= 0f)
        {
            Player_StateMachine.changeState(States[5]);
            Debug.Log("Player Dead");
        }
        else
        {
            Player_StateMachine.changeState(States[4]);

        }
    }
}

public class Dash_Values
{
    public float Current_DashRecovery = 0f;
    public float Current_DashTime = 0f;
    public int Current_DashAmount = 0;

    public void ResetValues()
    {
        Current_DashRecovery = 0f;
        Current_DashTime = 0f;
        Current_DashAmount = 0;
    }
}

public class Attack_Values
{
    public int comboCount = 0;

    public void ResetValues()
    {
        comboCount = 0;
    }
}


//For Interactions ---
public static class PlayerInteractionBase
{
    public static InteractionBase CallInteractionCheck()
    {
        Collider2D[] Interactable_Objects = Physics2D.OverlapCircleAll(PlayerController.Get_Controller.Get_PlayerRB.position, PlayerController.Get_Controller.Interaction_Distance, PlayerController.Get_Controller.Interaction_Layer);

        float DistanceToNearestObject = PlayerController.Get_Controller.Interaction_Distance;
        Collider2D ActiveObject = null;

        if (Interactable_Objects.Length != 0)
        {
            foreach (Collider2D element in Interactable_Objects)
            {
                if (Vector2.Distance(element.transform.position, PlayerController.Get_Controller.Get_PlayerRB.position) <= DistanceToNearestObject)
                {
                    DistanceToNearestObject = Vector2.Distance(element.transform.position, PlayerController.Get_Controller.Get_PlayerRB.position);
                    if (ActiveObject != element)
                        ActiveObject = element;
                }
            }

            if (ActiveObject != null)
                return ActiveObject.GetComponent<InteractionBase>();
            else
                return null;
        }
        else
        {
            return null;
        }
    }
}