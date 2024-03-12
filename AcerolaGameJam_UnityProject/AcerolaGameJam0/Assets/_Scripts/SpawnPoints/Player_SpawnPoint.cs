using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SpawnPoint : Interactable
{
    private bool PlayExit = false;
    public UIInteractData Interact_Instructions;

    protected virtual void Awake()
    {
        base.Awake();
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
        PlayExit = true;

        Respawn_Data Data = new Respawn_Data(transform.position);
        GameManager._GameManager.Set_Spawn(Data);

        Debug.Log("Spawn Point Set");

        if (Get_InteractionBase.Get_TutorialChannel != null)
        {
            Get_InteractionBase.Get_TutorialChannel.RaiseEvent_ShowTutorial(Get_InteractionBase.Get_TutorialData);
            Get_InteractionBase.Get_TutorialChannel = null;
        }
    }
}

public class Respawn_Data
{
    public int Scene_Index;
    public Vector2 Respawn_Location;

    public Respawn_Data(Vector2 position)
    {
        Scene_Index = SceneManagement.Get_SceneIndex();
        Respawn_Location = position;
    }

    //Player Health --- Player HealthKits --- Etc.
}
