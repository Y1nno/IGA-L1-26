using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ChefPrompts", menuName = "Scriptable Objects/ChefPrompts")]
public class ChefPrompts : ScriptableObject
{
    public string promptText;
    public string button1Text;
    public string button2Text;
    public UnityEvent button1Results;
    public UnityEvent button2Results;
}
