using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallTutorial : MonoBehaviour
{
    [SerializeField] private TutorialUI_Channel Channel;
    [SerializeField] private TutorialData Data;

    public void Call_TutorialOnFinish()
    {
        Channel.RaiseEvent_ShowTutorial(Data);
    }
}
