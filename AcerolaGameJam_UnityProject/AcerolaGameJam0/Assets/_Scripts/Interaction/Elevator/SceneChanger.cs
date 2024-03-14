using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : Interactable
{
    public int sceneIndex;
    [SerializeField] private GenericCallChannel Set_Target;

    protected override void On_Enter()
    {

    }

    protected override void On_Exit()
    {

    }

    protected override void On_Interact()
    {
        GameManager._GameManager.SceneChange_SetTarget = Set_Target;
        SceneManagement.ChangeScene(sceneIndex);
    }
}
