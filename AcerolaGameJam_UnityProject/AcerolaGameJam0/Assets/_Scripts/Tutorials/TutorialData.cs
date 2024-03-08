using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Tutorial Data", menuName = "Tutorials/Tutorial Data")]
public class TutorialData : ScriptableObject
{
    public string Mechanic_Title;
    public Sprite Key_Input, GamePad_Input;
    public VideoClip Tutorial_Clip;
}
