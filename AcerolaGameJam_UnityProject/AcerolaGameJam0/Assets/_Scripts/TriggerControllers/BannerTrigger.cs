using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerTrigger : MonoBehaviour
{
    [SerializeField] private string Banner_Text;
    [SerializeField] private UICallChannelString Banner_Show;
    [SerializeField] private UICallChannel Banner_Hide;

    [SerializeField] private CameraTargetChannel CameraTargetChannel;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera TargetCamera;

    //Hard Set ---
    [SerializeField] private float BannerTime;
    [SerializeField] private bool HoldPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Banner_Show != null)
            {
                Banner_Show.RaiseEvent(Banner_Text);
                if (TargetCamera != null)
                    CameraTargetChannel.RaiseEvent_OnEnter(TargetCamera);

                StartCoroutine(BannerTimer());

                if (HoldPlayer)
                    PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[0]);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Banner_Show = null;
        }
    }

    public void OnBanner_Exit()
    {
        if (TargetCamera != null)
            CameraTargetChannel.RaiseEvent_OnExit(TargetCamera);

        Banner_Hide.RaiseEvent();
        Banner_Hide = null;

        if (HoldPlayer)
            PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
    }

    public IEnumerator BannerTimer()
    {
        yield return new WaitForSecondsRealtime(BannerTime);
        OnBanner_Exit();
    }
}
