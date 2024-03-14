using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float Time;
    void Awake()
    {
        Destroy(gameObject, Time);
    }
}
