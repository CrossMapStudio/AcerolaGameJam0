using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Menu : MonoBehaviour
{
    public void Start_Game()
    {
        SceneManagement.ChangeScene(1);
    }

    public void Exit_Application()
    {
        Application.Quit();
    }
}
