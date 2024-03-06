using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZoneTrigger : MonoBehaviour
{
    [SerializeField] private CameraTargetChannel CameraTargetChannel;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera TargetCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (TargetCamera != null)
                CameraTargetChannel.RaiseEvent_OnEnter(TargetCamera);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (TargetCamera != null)
                CameraTargetChannel.RaiseEvent_OnExit(TargetCamera);
        }
    }
}
