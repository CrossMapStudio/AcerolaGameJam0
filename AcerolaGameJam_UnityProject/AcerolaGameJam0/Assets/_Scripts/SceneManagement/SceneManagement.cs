using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagement
{
    public static void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
