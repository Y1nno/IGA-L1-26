using UnityEngine;

public class LeaveDecisionExecutor : MonoBehaviour
{
    public void ExecuteDecision(bool leaving)
    {
        if (leaving)
        {
            DialogueManager.Instance.StartDialogue(DialogueInstances.Leaving);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(DialogueInstances.Staying);
        }
    }
}
