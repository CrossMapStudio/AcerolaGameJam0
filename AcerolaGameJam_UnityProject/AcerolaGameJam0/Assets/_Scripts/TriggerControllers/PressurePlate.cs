using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    //Channel to Connect to Time Fix Events ---
    [SerializeField] private GenericCallChannel ActivateChannel;
    [SerializeField] private GenericCallChannel Channel;
    [SerializeField] private bool PoweredOn = true;

    [SerializeField] private Animator _Anim;
    [SerializeField] private string OnPowerAnimationName, OnTriggerEnterAnimationName;

    private void Awake()
    {
        if (ActivateChannel != null)
            ActivateChannel.OnEventRaised.AddListener(PowerOn);
    }

    //Pressure Plates --- Call Once?
    protected void PowerOn()
    {
        PoweredOn = true;
        if (_Anim != null)
        {
            _Anim.Play(OnPowerAnimationName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Channel != null && PoweredOn)
            {
                Channel.RaiseEvent();
                //Play Anim ---
                if (_Anim != null)
                    _Anim.Play(OnTriggerEnterAnimationName);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
