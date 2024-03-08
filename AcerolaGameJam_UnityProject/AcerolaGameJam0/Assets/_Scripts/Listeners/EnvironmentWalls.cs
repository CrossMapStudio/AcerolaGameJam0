using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentWalls : MonoBehaviour
{
    [SerializeField] private GenericCallChannel Channel;
    
    [SerializeField] private Animator _Anim;
    [SerializeField] private string DropWallAnimationName;

    private void Awake()
    {
        if (Channel != null)
            Channel.OnEventRaised.AddListener(DropWall);
    }

    public void DropWall()
    {
        if (_Anim != null)
            _Anim.Play(DropWallAnimationName);
        else
            gameObject.SetActive(false);
    }
}
