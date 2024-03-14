using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_Medkit : Interactable
{
    public UIInteractData Interact_Instructions;
    //Different for Each Medkit --- Can't Generate Guid because the Instance will Change on Awake for Scene Switch ---
    private string Medkit_ID;
    [SerializeField] private GenericCallChannel GameManagerReset_LevelChannel;

    protected virtual void Awake()
    {
        base.Awake();
        GameManagerReset_LevelChannel.OnEventRaised.AddListener(Reset_Medkit);
    }

    protected void Start()
    {
        Medkit_ID = (transform.position.x.ToString() + " " + transform.position.y.ToString() + SceneManagement.Get_SceneIndex().ToString());

        if (GameManager._GameManager.Get_Medkit_CollectionStatus.ContainsKey(Medkit_ID))
        {
            if (GameManager._GameManager.Get_Medkit_CollectionStatus[Medkit_ID] || GameManager._GameManager.Temporary_TillSaveMedkitData.Contains(Medkit_ID))
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            GameManager._GameManager.Get_Medkit_CollectionStatus.Add(Medkit_ID, false);
        }

    }

    public void Reset_Medkit()
    {
        if (GameManager._GameManager.Get_Medkit_CollectionStatus.ContainsKey(Medkit_ID))
        {
            if (GameManager._GameManager.Get_Medkit_CollectionStatus[Medkit_ID] || GameManager._GameManager.Temporary_TillSaveMedkitData.Contains(Medkit_ID))
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            GameManager._GameManager.Get_Medkit_CollectionStatus.Add(Medkit_ID, false);
        }
    }

    protected override void On_Enter()
    {
        //Do Nothing
        Interact_Call.RaiseEvent(Interact_Instructions);
    }

    protected override void On_Exit()
    {
        Interact_Call.RaiseEvent(Interact_Instructions);
    }

    protected override void On_Interact()
    {
        if (Get_InteractionBase.Get_TutorialChannel != null)
        {
            Get_InteractionBase.Get_TutorialChannel.RaiseEvent_ShowTutorial(Get_InteractionBase.Get_TutorialData);
            Get_InteractionBase.Get_TutorialChannel = null;
            return;
        }

        if (PlayerController.Get_Controller.Current_MedicalCount < PlayerController.Get_Controller.Med_CountLimit)
        {
            PlayerController.Get_Controller.Current_MedicalCount++;
            GameManager._GameManager.Temporary_TillSaveMedkitData.Add(Medkit_ID);
            gameObject.SetActive(false);
        }
    }
}
