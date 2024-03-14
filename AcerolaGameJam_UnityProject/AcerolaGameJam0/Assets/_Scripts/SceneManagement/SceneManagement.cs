using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagement
{
    public static int Get_SceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
        GameManager._GameManager.Wait_SceneChange(sceneIndex);
    }
}
