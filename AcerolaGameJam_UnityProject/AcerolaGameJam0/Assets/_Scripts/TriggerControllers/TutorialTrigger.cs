using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TutorialData Tutorial_Data;
    [SerializeField] private TutorialUI_Channel Tutorial;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Tutorial != null)
            {
                Tutorial.RaiseEvent_ShowTutorial(Tutorial_Data);
                Tutorial = null;
            }
        }
    }
}
