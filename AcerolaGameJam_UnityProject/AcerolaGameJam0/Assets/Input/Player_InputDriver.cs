using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_InputDriver : SingletonPersistent<Player_InputDriver>
{
    #region Inputs
    //Subscribe through the Player Controller ---
    private Player_Input Input_Controller;
    public Player_Input Get_Input => Input_Controller;

    public static Player_InputDriver Input_Driver;

    private static InputAction Movement;
    public static InputAction Get_Movement => Movement;

    private static InputAction Dash;
    public static InputAction Get_Dash => Dash;

    private static InputAction Interact;
    public static InputAction Get_Interact => Interact;

    private static InputAction Inventory;
    public static InputAction Get_Inventory => Inventory;

    private static InputAction Attack_Light;
    public static InputAction Get_AttackLight => Attack_Light;

    private static Vector2 StoredDirection = new Vector2(1, 0);
    public static Vector2 Get_StoredDirection => StoredDirection;

    private static InputAction Healing;
    public static InputAction Get_Healing => Healing;

    private static InputAction Continue;
    public static InputAction Get_Continue => Continue;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        //Create the Input System ---
        Input_Driver = this;
        //Create a new instance for the Input ---
        Input_Controller = new Player_Input();
        Movement = Input_Controller.Player.Movement;
        Dash = Input_Controller.Player.Dash;
        Attack_Light = Input_Controller.Player.Attack;
        Interact = Input_Controller.Player.Interact;
        Healing = Input_Controller.Player.Healing;

        //Death Screen
        Continue = Input_Controller.DeathScreen.Continue;
    }

    public void OnEnable()
    {
        //Enable the Input ---
        Movement.Enable();
        Dash.Enable();
        Attack_Light.Enable();
        Interact.Enable();
        Healing.Enable();

        Continue.Enable();
    }

    public void OnDisable()
    {
        //Enable the Input ---
        Movement.Disable();
        Dash.Disable();
        Attack_Light.Disable();
        Interact.Disable();
        Healing.Disable();

        Continue.Disable();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Movement.Dispose();
        Dash.Dispose();
        Attack_Light.Dispose();
        Interact.Dispose();
        Healing.Dispose();

        Continue.Dispose();
    }

    public void Update()
    {
        Vector2 MovementVector = Movement.ReadValue<Vector2>();
        StoredDirection = MovementVector != Vector2.zero ? MovementVector : StoredDirection;
    }

    public static void ChangeInputMap_DeathScreen()
    {
        Input_Driver.Get_Input.Player.Disable();
        Input_Driver.Get_Input.DeathScreen.Enable();
    }

    public static void ChangeInputMap_PlayerControls()
    {
        Input_Driver.Get_Input.Player.Enable();
        Input_Driver.Get_Input.DeathScreen.Disable();
    }
}
