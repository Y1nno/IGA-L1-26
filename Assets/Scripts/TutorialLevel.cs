using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    public DialogueSO startingDialogue;
    public DialogueSO knifeDialogue;
    public DialogueSO breadDialogue;
    public DialogueSO firstOrderResolvedDialogue;
    public DialogueSO approvalChangeDialogue;
    private bool knifeGrabbed = false;
    private bool breadPlated = false;
    private bool firstOrderResolved = false;
    private bool approvalChanged = false;

    void Start()
    {
        DialogueManager.Instance.StartDialogue(startingDialogue);
    }

    public void ProcessLevelSignal(LevelSignal signal)
    {
        switch (signal)
        {
            case LevelSignal.GrabbedKnife:
                if (knifeGrabbed) { return; }
                DialogueManager.Instance.StartDialogue(knifeDialogue);
                knifeGrabbed = true;
                break;
            case LevelSignal.AddedBreadToPlate:
                if (breadPlated) { return; }
                breadPlated = true;
                DialogueManager.Instance.StartDialogue(breadDialogue);
                break;
            case LevelSignal.ResolvedFirstOrder:
                if (firstOrderResolved) { return; }
                firstOrderResolved = true;
                DialogueManager.Instance.StartDialogue(firstOrderResolvedDialogue);
                break;
            case LevelSignal.AffectionChange:
                if (approvalChanged) { return; }
                approvalChanged = true;
                AffectionManager.Instance.AffectionBar.gameObject.SetActive(true);
                DialogueManager.Instance.StartDialogue(approvalChangeDialogue);
                break;
        }
    }
}
