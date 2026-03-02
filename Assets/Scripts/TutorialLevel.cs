using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    public DialogueSO startingDialogue;
    public DialogueSO knifeDialogue;
    public DialogueSO breadDialogue;
    public DialogueSO firstOrderResolvedDialogue;
    private bool knifeGrabbed = false;
    private bool breadPlated = false;
    private bool firstOrderResolved = false;

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
        }
    }
}
