using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public int SceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManagement.ChangeScene(SceneIndex);
    }
}
