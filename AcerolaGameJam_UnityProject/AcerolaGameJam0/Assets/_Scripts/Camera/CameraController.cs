using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera Camera;
    private CinemachineBasicMultiChannelPerlin Noise_Profile;
    private float Smooth_Value = 1f;

    #region Camera Channels
    [SerializeField] private CameraShake_Channel CameraShakeChannel;
    [SerializeField] private CameraTargetChannel CameraTargetChannel;
    #endregion

    private void Awake()
    {
        Camera = GetComponent<CinemachineVirtualCamera>();
        Noise_Profile = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CameraShakeChannel.OnEventRaised.AddListener(InitializeCameraShake);
        CameraTargetChannel.OnEventRaised_Enter.AddListener(ChangeCameraTarget);
        CameraTargetChannel.OnEventRaised_Exit.AddListener(ResetCameraTarget);
    }

    private void Update()
    {
        if (Noise_Profile != null)
        {
            if (Noise_Profile.m_AmplitudeGain != 0)
                Noise_Profile.m_AmplitudeGain = Mathf.Lerp(Noise_Profile.m_AmplitudeGain, 0f, Smooth_Value * Time.deltaTime);

            if (Noise_Profile.m_FrequencyGain != 0)
                Noise_Profile.m_FrequencyGain = Mathf.Lerp(Noise_Profile.m_FrequencyGain, 0f, Smooth_Value * Time.deltaTime);
        }
    }

    private void InitializeCameraShake(float Magnitude, float Frequency, float Smooth)
    {
        Noise_Profile.m_AmplitudeGain = Magnitude;
        Noise_Profile.m_FrequencyGain = Frequency;

        Smooth_Value = Smooth;
    }

    private void ChangeCameraTarget(Cinemachine.CinemachineVirtualCamera Target)
    {
        Camera.Priority = 0;
        Noise_Profile = Target.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Target.Priority = 1;
    }

    private void ResetCameraTarget(Cinemachine.CinemachineVirtualCamera Target)
    {
        Camera.Priority = 1;
        Noise_Profile = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Target.Priority = 0;
    }
}
