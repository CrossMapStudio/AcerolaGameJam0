using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManagement.ChangeScene(1);
    }
}
