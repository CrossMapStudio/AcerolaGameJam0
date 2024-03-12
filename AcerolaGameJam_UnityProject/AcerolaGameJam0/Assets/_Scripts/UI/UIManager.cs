using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class UIManager : SingletonPersistent<UIManager>
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

    [SerializeField] private UICallChannelFloat Update_Health;

    [SerializeField] private GenericCallChannel Dash_Channel;
    [SerializeField] private GenericCallChannel Death_Channel;
    [SerializeField] private GenericCallChannel Respawn_Channel;

    [SerializeField] private UICallChannelFloat Dash_Update;
    [SerializeField] private UICallChannelInteract Interact_Channel;

    [SerializeField] private GenericCallChannel Banner_Hide;
    [SerializeField] private UICallChannelString Banner_Show;

    #region Tutorial
    [SerializeField] private TutorialUI_Channel Tutorial_Channel;
    [SerializeField] private GameObject Tutorial_UIContainer;
    [SerializeField] private VideoPlayer Tutorial_Player;
    [SerializeField] private Image KeyboardInput, GamepadInput;
    [SerializeField] private TMP_Text Mechanic_Title;
    #endregion

    #region Death Screen
    [SerializeField] private Animator Death_ScreenAnimator;
    [SerializeField] private string Death_ScreenShowAnimationName, Death_ScreenHideAnimationName;
    #endregion


    protected override void Awake()
    {
        Update_Health.OnEventRaised.AddListener(UpdateHealth);
        Dash_Channel.OnEventRaised.AddListener(UseDashNode);
        Dash_Update.OnEventRaised.AddListener(UpdateDashNode);
        Interact_Channel.OnEventRaised.AddListener(ToggleInteract);
        Banner_Show.OnEventRaised.AddListener(DisplayBanner);
        Banner_Hide.OnEventRaised.AddListener(HideBanner);

        Tutorial_Channel.OnEventRaised.AddListener(ShowTutorial);
        Banner_Anim.gameObject.SetActive(true);

        Death_Channel.OnEventRaised.AddListener(Show_DeathScreen);
        Respawn_Channel.OnEventRaised.AddListener(Continue_Game);

        base.Awake();
    }

    public void UpdateHealth(float currentValue)
    {
        if (currentValue <= 0)
            HealthBar.fillAmount = 0f;
        else
        HealthBar.fillAmount = currentValue / 100f;
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

    public void ShowTutorial(TutorialData data)
    {
        Player_InputDriver.Get_Interact.performed += HideTutorial;
        PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[0]);

        //Show the tutorial ---
        Tutorial_UIContainer.SetActive(true);
        Tutorial_Player.clip = data.Tutorial_Clip;
        KeyboardInput.sprite = data.Key_Input;
        GamepadInput.sprite = data.GamePad_Input;
        Mechanic_Title.text = data.Mechanic_Title;
    }

    public void HideTutorial(InputAction.CallbackContext context)
    {
        Player_InputDriver.Get_Interact.performed -= HideTutorial;
        Tutorial_UIContainer.SetActive(false);
        PlayerController.Get_Controller.Get_StateMachine.changeState(PlayerController.Get_Controller.Get_States[1]);
    }

    public void Show_DeathScreen()
    {
        Death_ScreenAnimator.Play(Death_ScreenShowAnimationName, 0, 0);
        Player_InputDriver.ChangeInputMap_DeathScreen();

        Player_InputDriver.Get_Continue.performed += GameManager._GameManager.Check_SceneRespawn;
    }

    public void Continue_Game()
    {
        Death_ScreenAnimator.Play(Death_ScreenHideAnimationName, 0, 0);
        Player_InputDriver.ChangeInputMap_PlayerControls();


        Player_InputDriver.Get_Continue.performed -= GameManager._GameManager.Check_SceneRespawn;
    }
}
