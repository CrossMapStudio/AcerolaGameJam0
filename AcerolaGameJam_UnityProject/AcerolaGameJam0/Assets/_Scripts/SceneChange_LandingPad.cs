using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange_LandingPad : MonoBehaviour
{
    [SerializeField] private GenericCallChannel Caller;
    private void Awake()
    {
        Caller.OnEventRaised.AddListener(Shift_Player);
    }

    private void Shift_Player()
    {
        //Shift the Player to the Landing Pad --- Set to Null ---
        PlayerController.Get_Controller.Get_PlayerRB.position = transform.position;
        Caller.OnEventRaised.RemoveListener(Shift_Player);
        GameManager._GameManager.SceneChange_SetTarget = null;
    }
}
