using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interaction Read Data", menuName = "Interact Data/Read")]
public class ReadMessage_SO : ScriptableObject
{
    [TextArea(2, 3)]
    [SerializeField] private string Message;
    public string Get_Message => Message;
}
