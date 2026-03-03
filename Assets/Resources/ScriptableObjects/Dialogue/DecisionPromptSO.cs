using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecisionPromptSO", menuName = "Scriptable Objects/DecisionPromptSO")]
public class DecisionPromptSO : ScriptableObject
{
    public string promptText;
    public List<DecisionSO> decisions = new List<DecisionSO>();
}
