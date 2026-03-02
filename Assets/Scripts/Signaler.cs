using UnityEngine;

public class TutorialSignaler : MonoBehaviour
{
    public GameObject LevelManager;
    public void Signal(LevelSignal signal)
    {
        LevelManager.GetComponent<TutorialLevel>().ProcessLevelSignal(signal);
    }
}

public enum LevelSignal
{
    GrabbedKnife,
    AddedBreadToPlate,
    ResolvedFirstOrder,
    AffectionChange
}