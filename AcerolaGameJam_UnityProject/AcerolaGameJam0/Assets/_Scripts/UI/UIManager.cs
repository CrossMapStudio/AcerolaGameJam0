using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Health UI
    public Image HealthBar;
    #endregion

    #region Dash UI
    public Image DashNode1, DashNode2; //Break this into controller?
    private Image TargetNode = null;
    #endregion

    #region Interact UI
    public GameObject ToolTip;
    public TMP_Text ToolTip_Text;
    #endregion

    #region Banner UI
    [SerializeField] private string ShowBannerAnimationName, HideBannerAnimationName;
    public Animator Banner_Anim;
    public TMP_Text Banner_Text;
    #endregion


    [SerializeField] private UICallChannel Dash_Channel;
    [SerializeField] private UICallChannelFloat Dash_Update;
    [SerializeField] private UICallChannelInteract Interact_Channel;

    [SerializeField] private UICallChannel Banner_Hide;
    [SerializeField] private UICallChannelString Banner_Show;

    private void Awake()
    {
        Dash_Channel.OnEventRaised.AddListener(UseDashNode);
        Dash_Update.OnEventRaised.AddListener(UpdateDashNode);
        Interact_Channel.OnEventRaised.AddListener(ToggleInteract);
        Banner_Show.OnEventRaised.AddListener(DisplayBanner);
        Banner_Hide.OnEventRaised.AddListener(HideBanner);

        Banner_Anim.gameObject.SetActive(true);
    }

    public void UseDashNode()
    {
        //Final Element --- //Play Animation and Set Dash Node Values to 0
        if (DashNode1.fillAmount == 1)
            DashNode1.fillAmount = 0;
        else
            DashNode2.fillAmount = 0;

        if (DashNode1.fillAmount != 1)
        {
            if (TargetNode != DashNode1)
                TargetNode = DashNode1;
        }
        else
        {
            if (TargetNode != DashNode2)
                TargetNode = DashNode2;
        }
    }

    public void UpdateDashNode(float Target)
    {
        if (TargetNode == null) return;

        TargetNode.fillAmount = PlayerController.Get_Controller.Get_DashValues.Current_DashRecovery / Target;
        if (TargetNode.fillAmount >= 1)
        {
            TargetNode.fillAmount = 1;
            if (DashNode1.fillAmount != 1)
            {
                TargetNode = DashNode1;
            }
            else if (DashNode2.fillAmount != 1)
            {
                TargetNode = DashNode2;
            }
            else
            {
                TargetNode = null;
            }
        }
    }

    public void ToggleInteract(UIInteractData _data)
    {
        if (ToolTip.activeSelf)
            ToolTip.SetActive(false);
        else {
            ToolTip.SetActive(true);
            ToolTip_Text.text = _data.Interact_Instructions;
        }
    }

    public void DisplayBanner(string _bannerText)
    {
        Banner_Anim.Play(ShowBannerAnimationName, 0, 0);
        Banner_Text.text = _bannerText;
    }

    public void HideBanner()
    {
        Banner_Anim.Play(HideBannerAnimationName, 0, 0);
    }
}
