using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy_Driver : MonoBehaviour
{
    [HideInInspector]
    public Guid EnemyInstance_Key;
    [HideInInspector] public Guid_Channel Spawn_PointLink;

    #region Enemy Data
    public float Starting_Health;
    [SerializeField] private float Detection_Radius;
    [SerializeField] private LayerMask Detection_Layers;

    //Rigidbody
    private Rigidbody2D Enemy_RB;
    public Rigidbody2D Get_EnemyRB => Enemy_RB;

    [SerializeField] private SpriteRenderer _Renderer;
    public SpriteRenderer Get_Renderer => _Renderer;

    //Player Detection
    [SerializeField] private bool _DetectPlayerImmediate;
    #endregion

    #region Information Pass
    public Vector3 Get_Position => transform.position;
    [HideInInspector]
    public Vector2 TargetPosition;

    private Transform Target;
    public Transform Get_Target => Target;

    [HideInInspector]
    public Vector2 Hit_Direction;
    #endregion

    #region Channels
    public Combat_Channel CombatChannel;
    public Combat_Channel Player_CombatChannel;
    [HideInInspector]
    public UnityEvent ChangeToHitState_Channel;
    #endregion

    #region StateMachine
    [HideInInspector]
    public Enemy_StateMachine EnemyStateMachine;
    #endregion

    #region States
    public List<Enemy_BaseState> Enemy_States;

    private int OffensiveStateSequences_Index = 0;
    public List<Offensive_StateGroup> Offensive_StateSequences;
    private Offensive_StateGroup Active_StateGroup;
    public Offensive_StateGroup Get_ActiveOffesiveStateGroup => Active_StateGroup;
    #endregion

    #region Animator
    [SerializeField] private Animator Enemy_Animator;
    public Animator Get_Animator => Enemy_Animator;
    #endregion

    #region Particle Systems
    public ParticleSystem Enemy_Hit_ParticleSystem;
    #endregion

    #region Animation Links
    [SerializeField] private AnimationChannel_Link AnimatorLinkChannels;
    
    [HideInInspector]
    public Animation_Invoke On_Init, On_Event, On_Finish;
    #endregion

    #region Ranged
    public Transform Modified_FirePoint;
    #endregion

    public void Awake()
    {
        EnemyStateMachine = new Enemy_StateMachine();
        Enemy_RB = GetComponent<Rigidbody2D>();

        for(int i = 0; i < Enemy_States.Count; i++)
        {
            //Create Instances for these ---
            Enemy_States[i] = Instantiate(Enemy_States[i]);
            Enemy_States[i].Driver = this;
        }

        for (int i = 0; i < Offensive_StateSequences.Count; i++)
        {
            Offensive_StateSequences[i].Initialize_States(this);
        }

        AnimatorLinkChannels.On_Init = Instantiate(AnimatorLinkChannels.On_Init);
        On_Init = AnimatorLinkChannels.On_Init;

        AnimatorLinkChannels.On_Event = Instantiate(AnimatorLinkChannels.On_Event);
        On_Event = AnimatorLinkChannels.On_Event;

        AnimatorLinkChannels.On_Finish = Instantiate(AnimatorLinkChannels.On_Finish);
        On_Finish = AnimatorLinkChannels.On_Finish;

        Active_StateGroup = Offensive_StateSequences[OffensiveStateSequences_Index];
    }

    public void SetTarget(Transform _Target)
    {
        Target = _Target;
    }

    public void Update()
    {
        EnemyStateMachine.executeStateUpdate();
    }

    public void FixedUpdate()
    {
        EnemyStateMachine.executeStateFixedUpdate();

        //Calls Once if Target in Range ---
        if (Target == null)
        {
            Collider2D Player_Collider = Physics2D.OverlapCircle(transform.position, Detection_Radius, Detection_Layers);
            //Detect Closest Target --- Set For Duration, Then Check Again??? --- For now just detect the Player
            if (Player_Collider != null)
            {
                EnemyStateMachine.changeState(Enemy_States[1]);
                Target = PlayerController.Get_Controller.Get_PlayerRB.transform;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject.tag == "Weapon")
        {
            CombatChannel.OnEventRaised.AddListener(Take_Damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            CombatChannel.OnEventRaised.RemoveListener(Take_Damage);
        }
    }

    public void LateUpdate()
    {
        EnemyStateMachine.executeStateLateUpdate();
    }

    public void Take_Damage(Vector2 direction, float damage)
    {
        float dot = Vector2.Dot(direction, ((Vector2)transform.position - PlayerController.Get_Controller.Get_PlayerRB.position).normalized);
        Debug.Log("Hit Dot Product: " + dot);

        if (dot >= .3f || Vector2.Distance(transform.position, PlayerController.Get_Controller.Get_PlayerRB.position) <= .25f)
        {
            TargetPosition = ((Vector2)transform.position - PlayerController.Get_Controller.Get_PlayerRB.position).normalized * 12f;
            Starting_Health -= damage;
            Debug.Log("Damage: " + damage);
            ChangeToHitState_Channel.Invoke();
        }

        if (Starting_Health <= 0f)
        {
            if (Spawn_PointLink != null)
                Spawn_PointLink.RaiseEvent(EnemyInstance_Key);
            Destroy(gameObject);
        }
    }

    public void Offensive_StateChange()
    {
        //Check Distance to Player --- Options within the Offensive States --- Track Last Picked Option ---
        var option = Get_ActiveOffesiveStateGroup.Get_OffensiveOption();

        if (option == null)
        {
            OffensiveStateSequences_Index++;
            if (OffensiveStateSequences_Index >= Offensive_StateSequences.Count)
                OffensiveStateSequences_Index = 0;

            EnemyStateMachine.changeState(Enemy_States[1]);
            return;
        }

        EnemyStateMachine.changeState(option);
    }
}

[Serializable]
public class Offensive_StateGroup
{
    public float Range;
    [HideInInspector]
    public int Previous_Index;
    public List<Enemy_BaseState> Offensive_Options;

    public void Initialize_States(Enemy_Driver Driver)
    {
        for (int i = 0; i < Offensive_Options.Count; i++)
        {
            //Create Instances for these ---
            Offensive_Options[i] = GameObject.Instantiate(Offensive_Options[i]);
            Offensive_Options[i].Driver = Driver;
        }
    }

    public Enemy_BaseState Get_OffensiveOption()
    {
        if (Previous_Index >= Offensive_Options.Count)
        {
            //Iterate to the Next Offensive Option ---
            Previous_Index = 0;
            return null;
        }

        var option = Offensive_Options[Previous_Index];
        Previous_Index++;
        return option;
    }
}
