using UnityEngine;


public abstract class Station : ScriptableObject
{
    [Header("Station Settings")]
    public StationType stationType;

    public abstract void Interact();
}
