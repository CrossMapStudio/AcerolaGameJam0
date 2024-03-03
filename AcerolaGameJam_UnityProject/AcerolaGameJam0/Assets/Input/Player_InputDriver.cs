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
    #endregion

    protected override void Awake()
    {
        base.Awake();
        //Create the Input System ---
        //Create a new instance for the Input ---
        Input_Controller = new Player_Input();
        Movement = Input_Controller.Player.Movement;
        Dash = Input_Controller.Player.Dash;
        Attack_Light = Input_Controller.Player.Attack;
        Interact = Input_Controller.Player.Interact;
    }

    public void OnEnable()
    {
        //Enable the Input ---
        Movement.Enable();
        Dash.Enable();
        Attack_Light.Enable();
        Interact.Enable();
    }

    public void Update()
    {
        Vector2 MovementVector = Movement.ReadValue<Vector2>();
        StoredDirection = MovementVector != Vector2.zero ? MovementVector : StoredDirection;
    }
}
