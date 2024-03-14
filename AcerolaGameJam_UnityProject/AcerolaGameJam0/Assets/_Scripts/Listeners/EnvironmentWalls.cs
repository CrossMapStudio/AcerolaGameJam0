using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentWalls : MonoBehaviour
{
    [SerializeField] private GenericBoolCallChannel Channel;
    
    [SerializeField] private Animator _Anim;
    [SerializeField] private string RaiseWallAnimationName, DropWallAnimationName;

    [SerializeField] private bool Default_Value;
    [SerializeField] private GenericCallChannel Level_Init;

    private void Awake()
    {
        if (Channel != null)
            Channel.OnEventRaised.AddListener(WallToggle);

        WallToggle(Default_Value);
        Level_Init.OnEventRaised.AddListener(Reset_Wall);
    }

    public void Reset_Wall()
    {
        WallToggle(Default_Value);
    }

    public void WallToggle(bool _Trigger)
    {
        if (_Trigger)
        {
            if (_Anim != null)
                _Anim.Play(RaiseWallAnimationName);
            else
                gameObject.SetActive(true);
        }
        else
        {
            if (_Anim != null)
                _Anim.Play(DropWallAnimationName);
            else
                gameObject.SetActive(false);
        }
    }
}
