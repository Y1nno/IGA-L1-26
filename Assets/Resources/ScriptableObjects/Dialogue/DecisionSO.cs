using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DecisionSO", menuName = "Scriptable Objects/DecisionSO")]
public class DecisionSO : ScriptableObject
{
    public string decisionText;
    public DialogueSO OnDecisionMade;
}
